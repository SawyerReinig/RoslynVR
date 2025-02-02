using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

public class RuntimeScriptLoader : MonoBehaviour
{
    [TextArea(3, 15)]
    public string generatedCode = @"
    using UnityEngine;

    public class DynamicScript : MonoBehaviour
    {
        void Start()
        {
            Debug.Log(""DynamicScript is running on "" + gameObject.name);
        }
    }";

    void Start()
    {
        callCompileAndRun(); 
    }

    [ContextMenu("Compile and Run")]
    void callCompileAndRun()
    {
        Type dynamicType = CompileAndLoadScript(generatedCode);
        if (dynamicType != null)
        {
            // GameObject obj = new GameObject("DynamicObject");
            gameObject.AddComponent(dynamicType);
        }
    }
    
    Type CompileAndLoadScript(string code)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        string assemblyName = Path.GetRandomFileName();

        // Locate required assembly paths
        string coreLibPath = Path.GetFullPath(typeof(object).Assembly.Location);
        string unityEnginePath = Path.GetFullPath(typeof(UnityEngine.Debug).Assembly.Location);
        string unityCoreModulePath = Path.GetFullPath(typeof(GameObject).Assembly.Location);
        string systemRuntimePath = Path.GetFullPath(typeof(Console).Assembly.Location);
        // 🔹 Locate Physics Module (for Rigidbody, Colliders, etc.)
        string unityPhysicsModulePath = Path.GetFullPath(typeof(Rigidbody).Assembly.Location);

        // 🔹 NEW: Locate netstandard.dll dynamically
        string netStandardPath = Directory.GetFiles(
            Path.GetDirectoryName(coreLibPath),
            "netstandard.dll",
            SearchOption.AllDirectories
        ).FirstOrDefault();

        if (string.IsNullOrEmpty(netStandardPath))
        {
            Debug.LogError("Could not find netstandard.dll. Make sure .NET Standard 2.1 is available.");
            return null;
        }
        // Add missing references
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(coreLibPath),
            MetadataReference.CreateFromFile(unityEnginePath),
            MetadataReference.CreateFromFile(unityCoreModulePath),
            MetadataReference.CreateFromFile(systemRuntimePath),
            MetadataReference.CreateFromFile(netStandardPath), // ✅ Fix for 'Object' missing error
            MetadataReference.CreateFromFile(unityPhysicsModulePath) // ✅ Fix for Rigidbody issue


        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using (var stream = new MemoryStream())
        {
            EmitResult result = compilation.Emit(stream);
            if (!result.Success)
            {
                Debug.LogError("Compilation failed:");
                foreach (var diagnostic in result.Diagnostics)
                    Debug.LogError(diagnostic.ToString());
                return null;
            }

            stream.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(stream.ToArray());
            return assembly.GetType("DynamicScript");
        }
    }
}

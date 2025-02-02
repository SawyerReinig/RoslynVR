using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

public class RuntimeScriptManager : MonoBehaviour
{

    public GameObject Target;

    public string SaveDllPath;
    public string generatedCode = @"
    using UnityEngine;

    public class Dynamic : MonoBehaviour
    {
        void Start()
        {
            Debug.Log(""DynamicScript is running on "" + gameObject.name);
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 2f;
        }
    }";

    void Start()
    {
        // callCompileAndRun(); 
    }

    [ContextMenu("Compile and Save DLL")]
    void callCompileAndRun()
    {
        // string dllPath = Path.Combine(Application.dataPath, "GeneratedScripts", "DynamicScript.dll");
        string dllPath = Path.Combine(SaveDllPath,"DynamicScript.dll");


        Type dynamicType = CompileAndSaveDLL(generatedCode, dllPath);
        if (dynamicType != null)
        { 
            // GameObject obj = new GameObject("DynamicObject");
            // obj.AddComponent(dynamicType);
            Target.AddComponent(dynamicType);
        }
    }
    
    Type CompileAndSaveDLL(string code, string outputPath)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        string assemblyName = Path.GetRandomFileName();

        // Locate required assembly paths dynamically
        string coreLibPath = Path.GetFullPath(typeof(object).Assembly.Location);
        string unityEnginePath = Path.GetFullPath(typeof(UnityEngine.Debug).Assembly.Location);
        string unityCoreModulePath = Path.GetFullPath(typeof(GameObject).Assembly.Location);
        string systemRuntimePath = Path.GetFullPath(typeof(Console).Assembly.Location);
        string unityPhysicsModulePath = Path.GetFullPath(typeof(Rigidbody).Assembly.Location);

        // 🔹 Locate netstandard.dll (needed for Unity .NET Standard compatibility)
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

        // Add all required references
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(coreLibPath),
            MetadataReference.CreateFromFile(unityEnginePath),
            MetadataReference.CreateFromFile(unityCoreModulePath),
            MetadataReference.CreateFromFile(systemRuntimePath),
            MetadataReference.CreateFromFile(netStandardPath),
            MetadataReference.CreateFromFile(unityPhysicsModulePath)
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // Ensure the output directory exists
        string directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        {
            EmitResult result = compilation.Emit(fileStream);
            if (!result.Success)
            {
                Debug.LogError("Compilation failed:");
                foreach (var diagnostic in result.Diagnostics)
                    Debug.LogError(diagnostic.ToString());
                return null;
            }
        }

        Debug.Log($"DLL successfully saved at: {outputPath}");

        // Load the compiled DLL back into memory
        Assembly assembly = Assembly.LoadFrom(outputPath);
        return assembly.GetType("Dynamic");
    }
}

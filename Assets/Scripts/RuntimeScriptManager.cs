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

    public string scriptID="";
    public string scriptName="";
    public string generatedCode = $@"
    using UnityEngine;

    public class Dynamic_ : MonoBehaviour
    {{
        void Start()
        {{
            Debug.Log(""DynamicScript is running on "" + gameObject.name);
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 2f;
        }}
    }}";

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



    public void RunScriptOntheObj(string code, GameObject target){


        print("Call run on object");
         string dllPath = Path.Combine(Application.streamingAssetsPath,"DynamicScript.dll");


             Type dynamicType = CompileAndSaveDLL(code, dllPath);
           // Type dynamicType = CompileAndLoadScript(code);

        if (dynamicType != null)
        { 
            // GameObject obj = new GameObject("DynamicObject");
            // obj.AddComponent(dynamicType);

             DynamicObj dynamicObj =target.GetComponent<DynamicObj>();


            dynamicObj.Log.Add(scriptName);
            dynamicObj.Log.Add(code);
            target.AddComponent(dynamicType);
        }

    }










       public void callCompileScript(string script)
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
    
    public Type CompileAndSaveDLL(string code, string outputPath)
    {
        string assemblyName = Path.GetFileNameWithoutExtension(outputPath);  // ✅ Match DLL filename

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        // Locate required assemblies
        string coreLibPath = Path.GetFullPath(typeof(object).Assembly.Location);
        string unityEnginePath = Path.GetFullPath(typeof(UnityEngine.Debug).Assembly.Location);
        string unityCoreModulePath = Path.GetFullPath(typeof(GameObject).Assembly.Location);
        string systemRuntimePath = Path.GetFullPath(typeof(Console).Assembly.Location);
        string unityPhysicsModulePath = Path.GetFullPath(typeof(Rigidbody).Assembly.Location);

        // Locate netstandard.dll (needed for Unity .NET Standard compatibility)
        string netStandardPath = Directory.GetFiles(
            Path.GetDirectoryName(coreLibPath),
            "netstandard.dll",
            SearchOption.AllDirectories
        ).FirstOrDefault();

        if (string.IsNullOrEmpty(netStandardPath))
        {
            Debug.LogError("❌ Could not find netstandard.dll. Make sure .NET Standard 2.1 is available.");
            return null;
        }

        // ✅ Add all required references
        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(coreLibPath),
            MetadataReference.CreateFromFile(unityEnginePath),
            MetadataReference.CreateFromFile(unityCoreModulePath),
            MetadataReference.CreateFromFile(systemRuntimePath),
            MetadataReference.CreateFromFile(netStandardPath),
            MetadataReference.CreateFromFile(unityPhysicsModulePath)
        };

        // ✅ Fix the assembly name issue
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,  // ✅ Ensures Unity loads the DLL properly
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        // ✅ Ensure the output directory exists
        string directory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // ✅ Compile and write the DLL
        using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        {
            EmitResult result = compilation.Emit(fileStream);
            if (!result.Success)
            {
                Debug.LogError("❌ Compilation failed:");
                foreach (var diagnostic in result.Diagnostics)
                {
                    Debug.LogError(diagnostic.ToString());
                }
                return null;
            }
        }

        Debug.Log($"✅ DLL successfully saved at: {outputPath}");

        // ✅ Load the compiled DLL into memory
        try
        {
            Assembly assembly = Assembly.LoadFrom(outputPath);
            Type scriptType = assembly.GetType("DynamicScript");  // Ensure class name matches

            if (scriptType != null)
            {
                Debug.Log("✅ Successfully loaded compiled script.");
                return scriptType;
            }
            else
            {
                Debug.LogError("❌ Could not find 'DynamicScript' in compiled DLL.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ Error loading DLL: {e.Message}");
            return null;
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

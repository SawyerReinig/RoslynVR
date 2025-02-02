using RoslynCSharp.Compiler;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RoslynCSharp.Example
{
    public class SawyerTest : MonoBehaviour
    {
        // Private
        private ScriptDomain domain = null;

        // Public
        public InputField codeInputField;      // Input field for user-defined code
        public Button compileRunButton;       // Button to compile and run code
        
        public Button editCodeButton;
        public GameObject codeEditorWindow;
        public Button codeEditorCloseButton;
        
        public AssemblyReferenceAsset[] assemblyReferences; // Assembly references for the script domain

        public TextAsset TestCode;

        private void Awake()
        {
            // compileRunButton.onClick.AddListener(CompileAndRunCode);
            // editCodeButton.onClick.AddListener(() => codeEditorWindow.SetActive(true));
            // codeEditorCloseButton.onClick.AddListener(() => codeEditorWindow.SetActive(false));


        }

        private void Start()
        {
            // Create a new script domain
            domain = ScriptDomain.CreateDomain("DynamicCodeDomain", true);
            codeInputField.text = TestCode.text;

            // Add assembly references
            foreach (AssemblyReferenceAsset reference in assemblyReferences)
            {
                domain.RoslynCompilerService.ReferenceAssemblies.Add(reference);
            }
        }

        private void CompileAndRunCode()
        {
            // Get the C# source code from the input field
            string sourceCode = codeInputField.text;

            try
            {
                // Compile the code
                ScriptType compiledType = domain.CompileAndLoadMainSource(sourceCode, ScriptSecurityMode.UseSettings, assemblyReferences);

                if (compiledType == null)
                {
                    HandleCompileErrors();
                    return;
                }

                // Dynamically invoke a method from the compiled script
                InvokeDynamicMethod(compiledType, "MyDynamicMethod");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}");
            }
        }

        private void HandleCompileErrors()
        {
            var compileResult = domain.RoslynCompilerService.LastCompileResult;

            if (compileResult == null || compileResult.Success)
            {
                Debug.LogError("Unknown error during compilation.");
                return;
            }

            foreach (var diagnostic in compileResult.Errors)
            {
                Debug.LogError($"Compile Error: {diagnostic.Message}");
            }
        }

        private void InvokeDynamicMethod(ScriptType scriptType, string methodName)
        {
            try
            {
                // Create an instance of the compiled class
                var instance = scriptType.CreateInstance();

                // Use reflection to get the type of the instance
                Type type = instance.GetType();

                // Find the method by name
                MethodInfo methodInfo = type.GetMethod(methodName);
                if (methodInfo == null)
                {
                    Debug.LogError($"Method '{methodName}' not found in the compiled script.");
                    return;
                }

                // Invoke the method on the instance
                methodInfo.Invoke(instance, null);

                Debug.Log($"Method '{methodName}' executed successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while executing method '{methodName}': {ex.Message}");
            }
        }

    }
}

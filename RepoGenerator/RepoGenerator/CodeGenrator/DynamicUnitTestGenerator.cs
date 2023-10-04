using Newtonsoft.Json.Schema;
using RepoGenerator.Interface;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGenerator.CodeGenrator
{
    public class DynamicUnitTestGenerator : IDynamicCodeGenerator
    {
        public Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema)
        {
            // Implement the unit test code generation logic here
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("DynamicNamespace");
            compileUnit.Namespaces.Add(codeNamespace);

            // Add the necessary using statements for unit testing
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("NUnit.Framework"));

            // Create the dynamic unit test class
            CodeTypeDeclaration testClass = new CodeTypeDeclaration("DynamicUnitTest");
            testClass.IsClass = true;
            testClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            // Add test methods for FetchData and SaveData
            CodeMemberMethod fetchTestMethod = new CodeMemberMethod();
            fetchTestMethod.Name = "TestFetchData";
            fetchTestMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            fetchTestMethod.CustomAttributes.Add(new CodeAttributeDeclaration("Test")); // Add NUnit Test attribute
            fetchTestMethod.ReturnType = new CodeTypeReference("void");
            fetchTestMethod.Statements.Add(new CodeSnippetStatement("var data = DynamicService.FetchData();"));
            fetchTestMethod.Statements.Add(new CodeSnippetStatement("// Assert data here (e.g., using NUnit assertions)"));
            testClass.Members.Add(fetchTestMethod);

            CodeMemberMethod saveTestMethod = new CodeMemberMethod();
            saveTestMethod.Name = "TestSaveData";
            saveTestMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            saveTestMethod.CustomAttributes.Add(new CodeAttributeDeclaration("Test")); // Add NUnit Test attribute
            saveTestMethod.ReturnType = new CodeTypeReference("void");
            saveTestMethod.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            saveTestMethod.Statements.Add(new CodeSnippetStatement("DynamicService.SaveData(data);"));
            saveTestMethod.Statements.Add(new CodeSnippetStatement("// Add assertions here (e.g., verify the data is saved correctly)"));
            testClass.Members.Add(saveTestMethod);

            // Add the dynamic unit test class to the namespace
            codeNamespace.Types.Add(testClass);

            Dictionary<string, CodeCompileUnit> compileUnits = new Dictionary<string, CodeCompileUnit>
        {
            { "DynamicUnitTest.cs", compileUnit }

        };
            return compileUnits;
        }
    }
}

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
    public class DynamicControllerGenerator : IDynamicCodeGenerator
    {
        public Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema)
        {
            // Implement the controller code generation logic here
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace("DynamicNamespace.Controllers");
            compileUnit.Namespaces.Add(codeNamespace);

            // Add the necessary using statements for the controller
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web.Http"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("DynamicNamespace.Services")); // Import the service interface

            // Create the dynamic controller class
            CodeTypeDeclaration controllerClass = new CodeTypeDeclaration("DynamicController");
            controllerClass.IsClass = true;
            controllerClass.BaseTypes.Add(new CodeTypeReference("ApiController"));

            // Add a private field to hold the service interface
            CodeMemberField serviceField = new CodeMemberField("IDynamicService", "_service");
            controllerClass.Members.Add(serviceField);

            // Create a constructor for the dynamic controller to inject the service interface
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression("IDynamicService", "service"));
            constructor.Statements.Add(new CodeSnippetStatement("_service = service;"));
            controllerClass.Members.Add(constructor);

            // Implement the controller actions
            CodeMemberMethod fetchMethod = new CodeMemberMethod();
            fetchMethod.Name = "FetchData";
            fetchMethod.Attributes = MemberAttributes.Public;
            fetchMethod.ReturnType = new CodeTypeReference("List<dynamic>"); // Assuming a list of dynamic objects for simplicity
            fetchMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpGet"));
            fetchMethod.Statements.Add(new CodeSnippetStatement("return _service.FetchData();"));
            controllerClass.Members.Add(fetchMethod);

            CodeMemberMethod saveMethod = new CodeMemberMethod();
            saveMethod.Name = "SaveData";
            saveMethod.Attributes = MemberAttributes.Public;
            saveMethod.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            saveMethod.CustomAttributes.Add(new CodeAttributeDeclaration("HttpPost"));
            saveMethod.Statements.Add(new CodeSnippetStatement("_service.SaveData(data);"));
            controllerClass.Members.Add(saveMethod);

            // Add the dynamic controller class to the namespace
            codeNamespace.Types.Add(controllerClass);

            Dictionary<string, CodeCompileUnit> compileUnits = new Dictionary<string, CodeCompileUnit>
        {
            { "DynamicController.cs", compileUnit }
        };

            return compileUnits;
        }
    }
}

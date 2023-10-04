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
    public class DynamicServiceGenerator : IDynamicCodeGenerator
    {
        public Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema)
        {
            // Implement the service code generation logic here
            CodeCompileUnit interfaceCompileUnit = new CodeCompileUnit();
            CodeNamespace interfaceNamespace = new CodeNamespace("DynamicNamespace.Services");
            interfaceCompileUnit.Namespaces.Add(interfaceNamespace);

            // Add the necessary using statements for the interface
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("System"));
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("DynamicNamespace.Repositories")); // Import the repository interface

            // Create the dynamic service interface
            CodeTypeDeclaration interfaceClass = new CodeTypeDeclaration("IDynamicService");
            interfaceClass.IsInterface = true;
            interfaceClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            // Add methods to the dynamic service interface (e.g., FetchData, SaveData, etc.)
            CodeMemberMethod fetchMethod = new CodeMemberMethod();
            fetchMethod.Name = "FetchData";
            fetchMethod.Attributes = MemberAttributes.Public;
            fetchMethod.ReturnType = new CodeTypeReference("List<dynamic>"); // Assuming a list of dynamic objects for simplicity
            interfaceClass.Members.Add(fetchMethod);

            CodeMemberMethod saveMethod = new CodeMemberMethod();
            saveMethod.Name = "SaveData";
            saveMethod.Attributes = MemberAttributes.Public;
            saveMethod.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            interfaceClass.Members.Add(saveMethod);

            // Add the dynamic service interface to the namespace
            interfaceNamespace.Types.Add(interfaceClass);

            // Create the dynamic service class namespace
            CodeCompileUnit classCompileUnit = new CodeCompileUnit();
            CodeNamespace classNamespace = new CodeNamespace("DynamicNamespace.Services");
            classCompileUnit.Namespaces.Add(classNamespace);

            // Add the necessary using statements for the class
            classNamespace.Imports.Add(new CodeNamespaceImport("System"));
            classNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            classNamespace.Imports.Add(new CodeNamespaceImport("DynamicNamespace.Repositories")); // Import the repository interface

            // Create the dynamic service class implementing the interface
            CodeTypeDeclaration serviceClass = new CodeTypeDeclaration("DynamicService");
            serviceClass.IsClass = true;
            serviceClass.BaseTypes.Add(new CodeTypeReference("IDynamicService"));

            // Add a private field to hold the repository interface
            CodeMemberField repoField = new CodeMemberField("IDynamicRepository", "_repository");
            serviceClass.Members.Add(repoField);

            // Create a constructor for the dynamic service to inject the repository interface
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression("IDynamicRepository", "repository"));
            constructor.Statements.Add(new CodeSnippetStatement("_repository = repository;"));
            serviceClass.Members.Add(constructor);

            // Implement the interface methods
            fetchMethod = new CodeMemberMethod();
            fetchMethod.Name = "FetchData";
            fetchMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            fetchMethod.ReturnType = new CodeTypeReference("List<dynamic>"); // Assuming a list of dynamic objects for simplicity
            fetchMethod.Statements.Add(new CodeSnippetStatement("return _repository.GetAll();"));
            serviceClass.Members.Add(fetchMethod);

            saveMethod = new CodeMemberMethod();
            saveMethod.Name = "SaveData";
            saveMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            saveMethod.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            saveMethod.Statements.Add(new CodeSnippetStatement("_repository.Save(data);"));
            serviceClass.Members.Add(saveMethod);

            // Add the dynamic service class to the namespace
            classNamespace.Types.Add(serviceClass);

            // Combine both compile units in a dictionary with keys representing file names
            Dictionary<string, CodeCompileUnit> compileUnits = new Dictionary<string, CodeCompileUnit>
        {
            { "IDynamicService.cs", interfaceCompileUnit },
            { "DynamicService.cs", classCompileUnit }
        };

            return compileUnits;
        }
    }
}

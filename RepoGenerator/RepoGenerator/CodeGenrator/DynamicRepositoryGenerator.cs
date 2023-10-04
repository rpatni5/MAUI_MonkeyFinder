using Newtonsoft.Json.Schema;
using RepoGenerator.Interface;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGenerator.CodeGenrator
{
    public class DynamicRepositoryGenerator : IDynamicCodeGenerator
    {
        private const string DatabaseName = "YourDatabaseName"; // Replace with your database name
        private const string CollectionName = "YourCollectionName"; // Replace with your collection name

        public Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema)
        {
            // Implement the repository code generation logic here
            CodeCompileUnit interfaceCompileUnit = GenerateInterfaceCode(schema);
            CodeCompileUnit classCompileUnit = GenerateClassCode(schema);

            // Combine both compile units in a dictionary with keys representing file names
            Dictionary<string, CodeCompileUnit> compileUnits = new Dictionary<string, CodeCompileUnit>
        {
            { "IDynamicRepository.cs", interfaceCompileUnit },
            { "DynamicRepository.cs", classCompileUnit }
        };

            return compileUnits;
        }

        private CodeCompileUnit GenerateInterfaceCode(JSchema schema)
        {
            CodeCompileUnit interfaceCompileUnit = new CodeCompileUnit();
            CodeNamespace interfaceNamespace = new CodeNamespace("DynamicNamespace.Repositories");
            interfaceCompileUnit.Namespaces.Add(interfaceNamespace);

            // Add the necessary using statements for the interface
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("System"));
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            interfaceNamespace.Imports.Add(new CodeNamespaceImport("MongoDB.Driver"));

            // Create the dynamic repository interface
            CodeTypeDeclaration interfaceClass = new CodeTypeDeclaration("IDynamicRepository");
            interfaceClass.IsInterface = true;
            interfaceClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            // Add methods to the dynamic repository interface (e.g., GetAll, Save, etc.)
            CodeMemberMethod getAllMethod = new CodeMemberMethod();
            getAllMethod.Name = "GetAll";
            getAllMethod.Attributes = MemberAttributes.Public;
            getAllMethod.ReturnType = new CodeTypeReference("List<dynamic>"); // Assuming a list of dynamic objects for simplicity
            interfaceClass.Members.Add(getAllMethod);

            CodeMemberMethod saveMethod = new CodeMemberMethod();
            saveMethod.Name = "Save";
            saveMethod.Attributes = MemberAttributes.Public;
            saveMethod.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            interfaceClass.Members.Add(saveMethod);

            // Add the dynamic repository interface to the namespace
            interfaceNamespace.Types.Add(interfaceClass);

            return interfaceCompileUnit;
        }

        private CodeCompileUnit GenerateClassCode(JSchema schema)
        {
            CodeCompileUnit classCompileUnit = new CodeCompileUnit();
            CodeNamespace classNamespace = new CodeNamespace("DynamicNamespace.Repositories");
            classCompileUnit.Namespaces.Add(classNamespace);

            // Add the necessary using statements for the class
            classNamespace.Imports.Add(new CodeNamespaceImport("System"));
            classNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            classNamespace.Imports.Add(new CodeNamespaceImport("MongoDB.Driver"));

            // Create the dynamic repository class implementing the interface
            CodeTypeDeclaration classType = new CodeTypeDeclaration("DynamicRepository");
            classType.IsClass = true;
            classType.BaseTypes.Add(new CodeTypeReference("IDynamicRepository"));

            // Implement the interface methods
            // Generate GetMongoCollection method
            CodeMemberMethod getMongoCollectionMethod = GenerateGetMongoCollectionMethod();
            classType.Members.Add(getMongoCollectionMethod);

            // Generate FetchDataFromMongoDB method
            CodeMemberMethod fetchDataFromMongoDBMethod = GenerateFetchDataFromMongoDBMethod();
            classType.Members.Add(fetchDataFromMongoDBMethod);

            // Generate SaveDataToMongoDB method
            CodeMemberMethod saveDataToMongoDBMethod = GenerateSaveDataToMongoDBMethod();
            classType.Members.Add(saveDataToMongoDBMethod);

            // Add the dynamic repository class to the namespace
            classNamespace.Types.Add(classType);

            return classCompileUnit;
        }

        private CodeMemberMethod GenerateGetMongoCollectionMethod()
        {
            var method = new CodeMemberMethod();
            method.Name = "GetMongoCollection";
            method.Attributes = MemberAttributes.Private | MemberAttributes.Static;
            method.ReturnType = new CodeTypeReference("IMongoCollection<dynamic>");
            method.Statements.Add(new CodeSnippetStatement($"var client = new MongoClient(GetMongoConnectionString());"));
            method.Statements.Add(new CodeSnippetStatement($"var database = client.GetDatabase(\"{DatabaseName}\");"));
            method.Statements.Add(new CodeSnippetStatement($"return database.GetCollection<dynamic>(\"{CollectionName}\");"));
            return method;
        }

        private CodeMemberMethod GenerateFetchDataFromMongoDBMethod()
        {
            var method = new CodeMemberMethod();
            method.Name = "FetchDataFromMongoDB";
            method.Attributes = MemberAttributes.Private | MemberAttributes.Static;
            method.ReturnType = new CodeTypeReference("List<dynamic>");
            method.Statements.Add(new CodeSnippetStatement($"var collection = GetMongoCollection();"));
            method.Statements.Add(new CodeSnippetStatement($"var documents = collection.Find(_ => true).ToList();"));
            method.Statements.Add(new CodeSnippetStatement("return documents;"));
            return method;
        }

        private CodeMemberMethod GenerateSaveDataToMongoDBMethod()
        {
            var method = new CodeMemberMethod();
            method.Name = "SaveDataToMongoDB";
            method.Attributes = MemberAttributes.Private | MemberAttributes.Static;
            method.Parameters.Add(new CodeParameterDeclarationExpression("dynamic", "data")); // Assuming a dynamic object for simplicity
            method.Statements.Add(new CodeSnippetStatement($"var collection = GetMongoCollection();"));
            method.Statements.Add(new CodeSnippetStatement($"collection.InsertOne(data);"));
            return method;
        }
    }
}

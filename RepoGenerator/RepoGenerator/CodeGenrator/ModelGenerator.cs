using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using Newtonsoft.Json.Linq;
using RepoGenerator.Interface;
using Newtonsoft.Json.Schema;
using Microsoft.CSharp;
using Newtonsoft.Json;

public class ModelGenerator : IDynamicCodeGenerator
{
    public Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema)
    {
        dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(schema.ToString());

        // Generate C# classes for the JSON model
        string className = "DynamicModel";
        CodeNamespace codeNamespace = new CodeNamespace("GeneratedNamespace");
        CodeTypeDeclaration classDeclaration = GenerateClass(jsonObj, className);

        codeNamespace.Types.Add(classDeclaration);

        // Generate C# code from the CodeDOM
        var provider = new CSharpCodeProvider();
        var options = new CodeGeneratorOptions();

        using (var writer = new StringWriter())
        {
            provider.GenerateCodeFromNamespace(codeNamespace, writer, options);
            var generatedCode = writer.ToString();

            Console.WriteLine(generatedCode);
            File.WriteAllText("D:\\Projects\\RepoGenerator\\RepoGenerator\\GeneratedClass\\DynamicModel.cs", generatedCode);
        }


        Dictionary<string, CodeCompileUnit> compileUnits = new Dictionary<string, CodeCompileUnit>();
        //{
        //    { "DynamicModel.cs", compileUnit }
        //};
        return compileUnits;
    }

    private static CodeTypeDeclaration GenerateClass(dynamic jsonObj, string className)
    {
        CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration(className)
        {
            IsClass = true,
            TypeAttributes = System.Reflection.TypeAttributes.Public
        };

        foreach (var property in jsonObj)
        {
            string propertyName = property.Name;
            JToken value = property.Value;

            if (value.Type == JTokenType.Object)
            {
                CodeTypeDeclaration nestedClass = GenerateClass(value, GetClassName(propertyName));
                classDeclaration.Members.Add(nestedClass);
                CodeMemberField propertyField = new CodeMemberField(nestedClass.Name, propertyName);
                propertyField.Attributes = MemberAttributes.Public;
                classDeclaration.Members.Add(propertyField);
            }
            else
            {
                string propertyType = GetCSharpType(value.Type);
                CodeMemberField propertyField = new CodeMemberField(propertyType, propertyName);
                propertyField.Attributes = MemberAttributes.Public;
                classDeclaration.Members.Add(propertyField);
            }
        }

        return classDeclaration;
    }

    // Convert JSON types to C# types
    private static string GetCSharpType(JTokenType type)
    {
        switch (type)
        {
            case JTokenType.Integer:
                return "int";
            case JTokenType.Float:
                return "double";
            case JTokenType.String:
                return "string";
            case JTokenType.Boolean:
                return "bool";
            case JTokenType.Date:
                return "DateTime";
            case JTokenType.TimeSpan:
                return "TimeSpan";
            default:
                return "object";
        }
    }

    // Generate a valid C# class name based on the property name
    private static string GetClassName(string propertyName)
    {
        return char.ToUpper(propertyName[0]) + propertyName.Substring(1);
    }


}

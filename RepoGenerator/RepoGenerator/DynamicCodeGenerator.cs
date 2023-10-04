using Newtonsoft.Json.Schema;
using RepoGenerator.CodeGenrator;
using RepoGenerator.Interface;
using System;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace RepoGenerator
{
    public class DynamicCodeGenerator
    {
        private static readonly Dictionary<string, IDynamicCodeGenerator> codeGenerators = new Dictionary<string, IDynamicCodeGenerator>
    {
        { "Repository", new DynamicRepositoryGenerator() },
        { "Service", new DynamicServiceGenerator() },
        { "Controller", new DynamicControllerGenerator() },
        { "UnitTest", new DynamicUnitTestGenerator() },
        { "Model", new ModelGenerator() }
    };

        public static void GenerateFromSchema(string jsonSchema, string outputDirectory)
        {
            // Parse JSON schema
            JSchema schema = JSchema.Parse(jsonSchema);

            // Generate dynamic code for repository, service, controller, and test case
            foreach (var codeGenerator in codeGenerators)
            {
                string codeType = codeGenerator.Key;
                IDynamicCodeGenerator generator = codeGenerator.Value;

                Dictionary<string, CodeCompileUnit> compileUnit = generator.GenerateFromSchema(schema);

                // Generate C# code from the compile unit
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";
                options.BlankLinesBetweenMembers = true;
                options.VerbatimOrder = true;

                // Create the output directory if it doesn't exist
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Export the generated code to files in the output directory
                string filePath = Path.Combine(outputDirectory, $"Dynamic{codeType}.cs");
                ExportToFile(provider, options, compileUnit, outputDirectory);
            }
        }

        private static void ExportToFile(CodeDomProvider provider, CodeGeneratorOptions options, Dictionary<string, CodeCompileUnit> compileUnits, string outputDirectory)
        {
            foreach (var codeGenerator in compileUnits)
            {
                var compileUnit = codeGenerator.Value;
                // Export the generated code to files in the output directory
                string filePath = Path.Combine(outputDirectory, codeGenerator.Key);
                // Create a StringWriter to output the generated code
                using (StringWriter writer = new StringWriter())
                {
                    // Generate the code and write it to the writer
                    provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

                    // Export the generated code to the file
                    File.WriteAllText(filePath, writer.ToString());
                    Console.WriteLine($"Generated file: {filePath}");
                }
            }
        }
    }
}

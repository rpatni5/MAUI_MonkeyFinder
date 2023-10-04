using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RepoGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string jsonSchema = @"{
            ""type"": ""object"",
            ""properties"": {
                ""Number"": { ""type"": ""string"" },
                ""RequestedDeviceType"": { ""type"": ""string"" },
                ""DeliveryMethod"": { ""type"": ""string"" },
                ""Customer"": {
                    ""type"": ""object"",
                    ""properties"": {
                        ""Contacts"": {
                            ""type"": ""object"",
                            ""properties"": {
                                ""FirstName"": { ""type"": ""string"" },
                                ""Name"": { ""type"": ""string"" },
                                ""Email"": { ""type"": ""string"" },
                                ""City"": { ""type"": ""string"" },
                                ""Address"": { ""type"": ""string"" },
                                ""MobilePhone"": { ""type"": ""string"" }
                            }
                        },
                        ""Name"": { ""type"": ""string"" },
                        ""Number"": { ""type"": ""string"" },
                        ""OverrideData"": { ""type"": ""boolean"" }
                    }
                },
                ""Vehicle"": {
                    ""type"": ""object"",
                    ""properties"": {
                        ""VIN"": { ""type"": ""string"" },
                        ""MakeModelCode"": { ""type"": ""string"" },
                        ""LicensePlate"": { ""type"": ""string"" },
                        ""Make"": { ""type"": ""string"" },
                        ""Model"": { ""type"": ""string"" },
                        ""YearOfInitialRegistration"": { ""type"": ""integer"" },
                        ""MotorType"": { ""type"": ""string"" },
                        ""OverrideData"": { ""type"": ""boolean"" }
                    }
                }
            }
        }";


        string outputDirectory = "D:\\Projects\\RepoGenerator\\RepoGenerator\\GeneratedClass"; // Provide the output directory where the files will be generated

        DynamicCodeGenerator.GenerateFromSchema(jsonSchema, outputDirectory);

        }
    }
}

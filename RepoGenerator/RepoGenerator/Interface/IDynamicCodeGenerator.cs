using Newtonsoft.Json.Schema;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoGenerator.Interface
{
    public interface IDynamicCodeGenerator
    {
        Dictionary<string, CodeCompileUnit> GenerateFromSchema(JSchema schema);
    }
}

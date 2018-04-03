using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaSON
{
    public static class JaSONTools
    {
      public static Boolean ValidateJSON(JObject jo, JsonSchema js )
      {
  
         return jo.IsValid(js);

      }
   }
}

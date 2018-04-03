using JaSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using PapaLegba;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Argonaut
{
   class Program
   {
      static void Main(string[] args)
      {
         /*// load from a string
         JsonSchema schema1 = JsonSchema.Parse(@"{'type':'object'}");

         // load from a file
         using (TextReader reader = File.OpenText(@"c:\schema\Person.json"))
         {
             JsonSchema schema2 = JsonSchema.Read(new JsonTextReader(reader));

             // do stuff
         }*/

         Jedi luke = new Jedi(100, "Luke Skywalker");
         AMQMessage amq = new AMQMessage();

         String json = JsonConvert.SerializeObject(luke);       
         Console.WriteLine("\nJaSON the jedi is; \n {0}\n", json);
         JObject jo = JObject.Parse(json);
         JsonSchema js = new JsonSchema();
         js.Type = JsonSchemaType.Object;
         js.Properties = new Dictionary<string, JsonSchema>
         {
             { "Name", new JsonSchema { Type = JsonSchemaType.String, Required = true} },
             { "Force", new JsonSchema {Type = JsonSchemaType.Integer , Required = true} }
         };
         Console.WriteLine("\nJaSON's schema is; \n {0}\n", js.ToString());

         String isValid = JaSONTools.ValidateJSON(jo, js) ? String.Format("{0} is a Valid", jo.ToString()) : String.Format("{0} is NOT Valid", jo.ToString());
         Console.WriteLine("{0}", isValid);

         json = JsonConvert.SerializeObject(amq);
         Console.WriteLine("\nJaSON the amq is; \n {0}\n", json);
         jo = JObject.Parse(json);
         isValid = JaSONTools.ValidateJSON(jo, js) ? String.Format("{0} is a Valid", jo.ToString()) : String.Format("{0} is NOT Valid", jo.ToString());

         Console.WriteLine("{0}", isValid);
         Console.Read();
      }
   }
}

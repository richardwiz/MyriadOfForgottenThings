using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PolymorphicJson.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PolymorphicJson
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Animal a = null;
            ListViewItem lvi = new ListViewItem();

            if (radioButton1.Checked)
            {
                a = new Dog() { breed = "Labrador", furLength = 1.5, landBased = true, numberOfLimbs = 4, weight = 24.0 };
                lvi.Text = "Dog";
                lvi.SubItems.Add(JsonConvert.SerializeObject(a, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            }));
            }
            if (radioButton2.Checked)
            {
                a = new Cat() { eyeColor = "Blue", hypoAllergenic = true, landBased = true, numberOfLimbs = 4, weight = 14.5 };
                lvi.Text = "Cat";
                lvi.SubItems.Add(JsonConvert.SerializeObject(a, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            }));
            }
            if (radioButton3.Checked)
            {
                a = new Robin() { eggIncubationTime = 6, gender = "Male", numberOfLimbs = 2, weight = 1.5 };
                lvi.Text = "Robin";
                lvi.SubItems.Add(JsonConvert.SerializeObject(a, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            }));
            }
            if (radioButton4.Checked)
            {
                a = new Eagle() { age = 9, eggIncubationTime = 9, numberOfLimbs = 2, weight = 12.0 };
                lvi.Text = "Eagle";
                lvi.SubItems.Add(JsonConvert.SerializeObject(a, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            }));
            }

            listView1.Items.Add(lvi);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            if (listView1.SelectedItems.Count == 1)
            {
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = String.Empty;
            string jsonAnimalString = textBox1.Text;
            try
            {
                var eventConverter = new AnimalConverter();
                var deseralizeSettings = new JsonSerializerSettings();
                deseralizeSettings.Converters.Add(eventConverter);
                deseralizeSettings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                deseralizeSettings.TypeNameHandling = TypeNameHandling.Auto;

                Animal someAnimal = JsonConvert.DeserializeObject<Animal>(jsonAnimalString, deseralizeSettings);
                textBox2.Text = someAnimal.ToString();

                Console.Write("Done");
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }
        }
    }

    #region Converters

    /// <summary>
    /// Custom converter to convert objects to and from JSON
    /// </summary>
    /// <typeparam name="T">The type of object being passed in</typeparam>
    public abstract class CustomJsonConverter<T> : JsonConverter
    {
        /// <summary>
        /// Abstract method which implements the appropriate create method
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jsonObject);

        /// <summary>
        /// Determines whether an instance of the current System.Type can be assigned from an instance of the specified Type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Reads JSON and returns the appropriate object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // load the json string
            var jsonObject = JObject.Load(reader);

            // instantiate the appropriate object based on the json string
            var target = Create(objectType, jsonObject);

            // populate the properties of the object
            serializer.Populate(jsonObject.CreateReader(), target);

            // return the object
            return target;
        }

        /// <summary>
        /// Creates the JSON based on the object passed in
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// The converter to use when deserializing animal objects
    /// </summary>
    public class AnimalConverter : CustomJsonConverter<Animal>
    {
        /// <summary>
        /// The class that will create Animals when proper json objects are passed in
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        protected override Animal Create(Type objectType, JObject jsonObject)
        {
            // examine the $type value
            string typeName = (jsonObject["$type"]).ToString();

            // based on the $type, instantiate and return a new object
            switch (typeName)
            {
                case "PolymorphicJson.Classes.Dog, PolymorphicJson":
                    return new Dog();
                case "PolymorphicJson.Classes.Cat, PolymorphicJson":
                    return new Cat();
                case "PolymorphicJson.Classes.Robin, PolymorphicJson":
                    return new Robin();
                case "PolymorphicJson.Classes.Eagle, PolymorphicJson":
                    return new Eagle();
                default:
                    return null;
            }
        }
    }



    #endregion
}

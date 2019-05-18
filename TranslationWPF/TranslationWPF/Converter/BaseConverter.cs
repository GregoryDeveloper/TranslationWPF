using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using TranslationWPF.Model;

namespace TranslationWPF.Converter
{
    public class BaseConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Language));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["ObjType"].Value<int>())
            {
                case (int)Language.Languages.English:
                    return JsonConvert.DeserializeObject<English>(jo.ToString(), SpecifiedSubclassConversion);
                case (int)Language.Languages.French:
                    return JsonConvert.DeserializeObject<French>(jo.ToString(), SpecifiedSubclassConversion);

                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}

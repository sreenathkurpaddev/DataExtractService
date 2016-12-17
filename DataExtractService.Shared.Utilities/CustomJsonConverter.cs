using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Shared.Utilities
{
    public class CustomJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        /// <summary>
        /// Implementation that overrides the base converter to provide more flexibility over De-Serilization
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            object targetObject = Activator.CreateInstance(objectType);
            foreach (PropertyInfo property in objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                JsonPropertyAttribute jsonProperty = property.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
                string jsonPath = jsonProperty != null ? jsonProperty.PropertyName : string.Empty;

                if (string.IsNullOrEmpty(jsonPath)) continue;

                if (jsonPath.Contains('|'))
                {
                    string[] jsonPathArray = jsonPath.Split('|');
                    if (jsonPathArray.Count() > 1)
                    {
                        jsonPath = jsonPathArray[0];
                        JToken validationToken = jObject.SelectToken(jsonPathArray[1]);
                        if (validationToken != null && validationToken.Type != JTokenType.Null)
                        {
                            object validationValue = validationToken.ToObject(typeof(bool), serializer);
                            if (!(bool)validationValue)
                            {
                                return null;
                            }
                        }
                    }
                }


                if (jsonPath.Contains('*'))
                {
                    List<JToken> tokens = jObject.SelectTokens(jsonPath).ToList();
                    if (tokens != null && tokens.Count > 0)
                    {
                        dynamic listValue = Activator.CreateInstance(property.PropertyType);
                        foreach (JToken listToken in tokens)
                        {
                            dynamic singleValue = listToken.ToObject(property.PropertyType.GetGenericArguments()[0], serializer);

                            listValue.Add(singleValue);
                        }
                        property.SetValue(targetObject, listValue, null);
                    }
                }

                else
                {

                    JToken currentToken = jObject.SelectToken(jsonPath);
                    if (currentToken != null && currentToken.Type != JTokenType.Null)
                    {
                        object currentValue = currentToken.ToObject(property.PropertyType, serializer);
                        property.SetValue(targetObject, currentValue, null);
                    }
                }
            }
            return targetObject;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using SceneModel.ContactAreas;

namespace SceneServices
{
    public class ContactAreaJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ContactArea toWrite = value as ContactArea;
            if (toWrite == null)
                throw new ArgumentNullException(nameof(value));
            writer.WriteValue(toWrite.AsString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string toConvert = reader.Value as string;
            if (toConvert == null)
                throw new InvalidOperationException("Failed to parse null string");
            return ContactArea.FromString(toConvert);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ContactArea).IsAssignableFrom(objectType);
        }
    }
}
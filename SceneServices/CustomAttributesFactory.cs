using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SceneModel;

namespace SceneServices
{
    public static class CustomAttributesFactory
    {
        public static JsonConverter JsonConverter => new CustomAttributesJsonConverter();

        /// <summary>
        /// returns null, if <see cref="customAttributes"/> is null
        /// </summary>
        /// <param name="customAttributes"></param>
        /// <returns></returns>
        public static JObject FromAttributes(this CustomAttributes customAttributes)
        {
            return customAttributes.ExtractJObject();
        }

        /// <summary>
        /// always return non-null
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        public static CustomAttributes ToAttributes(this JObject jo)
        {
            return new CustomAttributesImplementation
            {
                Data = jo ?? new JObject(),
            };
        }

        public static CustomAttributes Merge(this CustomAttributes src, CustomAttributes other, bool inPlace = false)
        {
            var retVal = src.ExtractJObject();
            if (!inPlace)
                retVal = (JObject)retVal.DeepClone(new JsonCloneSettings { CopyAnnotations = false });

            var o = other.ExtractJObject();
            retVal.Merge(o,
                new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Replace, // TODO: how to handle arrays?
                    PropertyNameComparison = StringComparison.OrdinalIgnoreCase,
                    MergeNullValueHandling = MergeNullValueHandling.Merge
                });
            return retVal.ToAttributes();
        }

        private sealed class CustomAttributesImplementation : CustomAttributes
        {
            public JObject Data;
        }

        private static JObject ExtractJObject(this object value)
        {
            var impl = value as CustomAttributesImplementation;
            return impl?.Data;
        }

        private static void WriteEmptyCustomAttributes(this JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        private class CustomAttributesJsonConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var jo = value.ExtractJObject();
                if (jo == null)
                    writer.WriteEmptyCustomAttributes();
                else
                    jo.WriteTo(writer, serializer.Converters.ToArray());
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var jo = reader.TokenType == JsonToken.Null ? null : JObject.Load(reader);
                var retVal = jo.ToAttributes();
                return retVal;
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(CustomAttributes).IsAssignableFrom(objectType);
            }
        }
    }
}
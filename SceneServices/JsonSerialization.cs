using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SceneModel;

namespace SceneServices
{
    public static class JsonSerialization
    {
        private static readonly IContractResolver resolver = new JsonContractResolver();
        private static readonly JsonConverter enumConverter = new StringEnumConverter();
        private static readonly JsonConverter contactAreaConverter = new ContactAreaJsonConverter();
        private static readonly JsonConverter customAttributesConverter = CustomAttributesFactory.JsonConverter;
        public static JsonSerializerSettings Default => new JsonSerializerSettings
        {
            ContractResolver = resolver,
            Converters = new List<JsonConverter>
            {
                enumConverter,
                contactAreaConverter,
                customAttributesConverter,
            }
        };

        public static JsonSerializerSettings Formatted => new JsonSerializerSettings
        {
            ContractResolver = resolver,
            Converters = new List<JsonConverter>
            {
                enumConverter,
                contactAreaConverter,
                customAttributesConverter,
            },
            Formatting = Formatting.Indented,
        };

        public static Scene ToScene(this string json)
        {
            var retVal = JsonConvert.DeserializeObject<Scene>(json, Default);
            retVal?.MakeAlive();
            return retVal;
        }

        public static ScenePatch ToScenePatch(this string json)
        {
            var retVal = JsonConvert.DeserializeObject<ScenePatch>(json, Default);
            return retVal;
        }
        public static string ToJson(this Scene scene, bool formatted = false)
        {
            return JsonConvert.SerializeObject(scene, formatted ? Formatted : Default);
        }

        public static JObject GetCustomAttributes(this Scene scene)
        {
            return scene.Custom.FromAttributes() ?? new JObject();
        }

        public static void SetCustomAttributes(this Scene scene, JObject data)
        {
            scene.Custom = data.ToAttributes();
        }
    }
}
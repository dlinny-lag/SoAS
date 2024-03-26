using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SceneModel;

namespace SceneServices
{
    public class JsonContractResolver : DefaultContractResolver
    {
        private static bool Ignoring(JsonProperty prop)
        {
            return prop.AttributeProvider.GetAttributes(typeof(IgnoreAttribute), true).Count > 0;
        }
        private static bool Mandatory(JsonProperty prop)
        {
            return prop.AttributeProvider.GetAttributes(typeof(MandatoryAttribute), true).Count > 0;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var retVal = base.CreateProperties(type, memberSerialization);
            retVal = retVal.Where(p => !Ignoring(p)).ToList();
            foreach (JsonProperty property in retVal)
            {
                if (Mandatory(property))
                    property.Writable = true;
            }

            return retVal;
        }
    }
}

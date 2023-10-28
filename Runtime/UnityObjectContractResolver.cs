using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QuickEye.ImportableAssets
{
    public class UnityObjectContractResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _ignoreProps = new HashSet<string>
        {
            "name", "hideFlags"
        };

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (_ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    public abstract class UnityObjectContractResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _ignoreProps = new HashSet<string>
        {
            "name", "hideFlags"
        };

        protected abstract JsonConverter<Object> UnityObjectConverter { get; }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            
            if (IsUnityEngineObject(property.PropertyType))
            {
                property.Converter = UnityObjectConverter;
            }

            if (_ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }

        private static bool IsUnityEngineObject(Type type)
        {
            if (type == null)
                return false;
            return typeof(UnityEngine.Object).IsAssignableFrom(type);
        }
    }
}
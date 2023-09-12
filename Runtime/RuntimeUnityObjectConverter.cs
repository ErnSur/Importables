using System;
using System.Reflection;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    internal class RuntimeUnityObjectConverter : JsonConverter<Object>
    {
        private const string InstanceIdKey = "instanceID";
        private static readonly MethodInfo FindObjectFromInstanceIDMethodInfo = GetFindObjectFromInstanceID();

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
                return null;
            reader.Read();
            var instanceId = reader.ReadAsInt32();
            return instanceId == null ? null : FindObjectFromInstanceID(instanceId.Value);
        }

        public override void WriteJson(JsonWriter writer, Object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(InstanceIdKey);
            writer.WriteValue(value.GetInstanceID());
            writer.WriteEndObject();
        }

        private static Object FindObjectFromInstanceID(int instanceId)
        {
            if (FindObjectFromInstanceIDMethodInfo == null)
                return null;
            return (Object)FindObjectFromInstanceIDMethodInfo.Invoke(null, new object[] { instanceId });
        }

        private static MethodInfo GetFindObjectFromInstanceID()
        {
            return typeof(Object)
                .GetMethod("FindObjectFromInstanceID",
                    BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
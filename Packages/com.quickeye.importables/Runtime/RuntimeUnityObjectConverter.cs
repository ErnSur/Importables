using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    internal class RuntimeUnityObjectConverter : JsonConverter<Object>
    {
        private const string InstanceIdKey = "instanceID";
        private static readonly MethodInfo FindObjectFromInstanceIDMethodInfo = GetFindObjectFromInstanceID();

        private bool _skipOverMe;

        public override bool CanWrite
        {
            get
            {
                if (_skipOverMe)
                {
                    _skipOverMe = false;
                    return false;
                }

                return true;
            }
        }

        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Path == "")
                return null;

            if (reader.TokenType != JsonToken.StartObject)
                return null;

            Object result = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value?.ToString();
                    reader.Read();
                    var instanceId = reader.Value;

                    if (name is "instanceID" && instanceId != null && reader.TokenType == JsonToken.Integer)
                    {
                        result = FindObjectFromInstanceID(Convert.ToInt32(instanceId));
                    }
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, Object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (writer.Path != "")
            {
                writer.WriteStartObject();
                writer.WritePropertyName(InstanceIdKey);
                writer.WriteValue(value.GetInstanceID());
                writer.WriteEndObject();
            }
            else
            {
                _skipOverMe = true;
                serializer.Serialize(writer, value);
            }
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
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor
{
    internal class EditorUnityObjectConverter : JsonConverter<Object>
    {
        public override Object ReadJson(JsonReader reader, Type objectType, Object existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value?.ToString();
                    var val = reader.ReadAsString();

                    if (name is "guid" && val != null)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(val);
                        var assetRef = AssetDatabase.LoadAssetAtPath<Object>(path);
                        return assetRef;
                    }
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, Object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var json = EditorJsonUtility.ToJson(new ReferenceHolder(value));
            var jObj = JObject.Parse(json);
            var valueToken = jObj[nameof(ReferenceHolder.obj)];
            WriteFormattedRawValue(writer, valueToken?.ToString());
        }

        private static void WriteFormattedRawValue(JsonWriter writer, string json)
        {
            if (json == null)
                writer.WriteRawValue(json);
            else
            {
                using var reader = new JsonTextReader(new StringReader(json));
                writer.WriteToken(reader);
            }
        }
        
        [Serializable]
        private class ReferenceHolder
        {
            public Object obj;
            public ReferenceHolder(Object obj) => this.obj = obj;
        }
    }
}
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
            string guid = null;
            long fileId = -1;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value?.ToString();
                    reader.Read();
                    var val = reader.Value?.ToString();

                    if (name is "guid" && val != null)
                    {
                        guid = val;
                    }
                    else if (name is "fileID" && val != null && reader.TokenType == JsonToken.Integer)
                    {
                        fileId = Convert.ToInt64(val);
                    }

                    if (result == null & guid != null && fileId != -1)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var assetsAtPath = AssetDatabase.LoadAllAssetsAtPath(path);
                        foreach (var asset in assetsAtPath)
                        {
                            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out var assetGuid,
                                    out long assetFileId)
                                && assetFileId == fileId && assetGuid == guid)
                                result = asset;
                        }
                    }
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, Object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (writer.Path != "")
            {
                var json = EditorJsonUtility.ToJson(new ReferenceHolder(value));
                var jObj = JObject.Parse(json);
                var valueToken = jObj[nameof(ReferenceHolder.obj)];
                WriteFormattedRawValue(writer, valueToken?.ToString());
            }
            else
            {
                _skipOverMe = true;
                serializer.Serialize(writer, value);
            }
        }

        private static void WriteFormattedRawValue(JsonWriter writer, string json)
        {
            if (json == null)
            {
                writer.WriteRawValue(json);
            }
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

            public ReferenceHolder(Object obj)
            {
                this.obj = obj;
            }
        }
    }
}
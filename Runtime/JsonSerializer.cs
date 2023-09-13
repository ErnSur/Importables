using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    /// <summary>
    /// Handles serialization of types unsupported by Unity. Unity Objects are serialized like in <see cref="UnityEngine.JsonUtility"/>
    /// </summary>
    public class JsonSerializer : TextSerializer
    {
        public JsonSerializerSettings Settings { get; set; } =
            new JsonSerializerSettings
            {
                Error = HandleError,
                ContractResolver = new RuntimeContractResolver()
            };

        public override Object FromText(string text, Type objectType)
        {
            var objectToOverwrite = ScriptableObject.CreateInstance(objectType);
            JsonConvert.PopulateObject(text, objectToOverwrite, Settings);
            return objectToOverwrite;
        }

        public override string ToText(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }

        private static void HandleError(object sender, ErrorEventArgs e)
        {
            var ex = e.ErrorContext.Error;
            if (ex is JsonSerializationException && ex.Message.Contains("Cannot deserialize the current "))
            {
                Debug.Log($"Hmmm {ex}");
                e.ErrorContext.Handled = true;
            }
        }
    }
}
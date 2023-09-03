using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor
{
    internal class EditorContractResolver : UnityObjectContractResolver
    {
        protected override JsonConverter<Object> UnityObjectConverter { get; } = new EditorUnityObjectConverter();
    }
}
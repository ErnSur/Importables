using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    internal class RuntimeContractResolver : UnityObjectContractResolver
    {
        protected override JsonConverter<Object> UnityObjectConverter { get; } = new RuntimeUnityObjectConverter();
    }
}
using JetBrains.Annotations;

namespace QuickEye.ImportableAssets.Editor
{
    /// <summary>
    /// Handles serialization of types unsupported by Unity. Unity Objects are serialized like in <see cref="UnityEditor.EditorJsonUtility"/>
    /// </summary>
    [PublicAPI]
    public class EditorJsonSerializer : JsonSerializer
    {
        public EditorJsonSerializer()
        {
            Settings.ContractResolver = new EditorContractResolver();
        }
    }
}
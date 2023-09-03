using UnityEngine;

namespace QuickEye.ImportableAssets.Samples.JsonImporter
{
    [CreateAssetMenu]
    public class ServerConfig : ScriptableObject
    {
        public string configVersion;
        public string token;
        public UnityDictionary<string, string> parameters;
        
        public Object asset;
    }
}

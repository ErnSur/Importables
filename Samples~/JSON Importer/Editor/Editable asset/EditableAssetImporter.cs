using System;
using QuickEye.ImportableAssets.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace QuickEye.ImportableAssets.Samples.JsonImporter.Editor
{
    /// <summary>
    /// Uses <see cref="EditorJsonSerializer"/>
    /// Unity Object references are serialized with <see cref="UnityEditor.EditorJsonUtility"/>
    /// </summary>
    [ScriptedImporter(1, FileExtension)]
    public class EditableAssetImporter : ScriptableObjectImporter
    {
        private const string FileExtension = ".editable-asset-example";

        [MenuItem("Assets/Create/Server Config - Editable Asset Example", false)]
        public static void CreateJsonAsset()
        {
            CreateTextAssetAction.StartFileNameEdit($"New Asset{FileExtension}", "{ }");
        }

        public override Type ScriptableObjectType => typeof(ServerConfig);
        public override ISerializer Serializer => new EditorJsonSerializer();
    }
}
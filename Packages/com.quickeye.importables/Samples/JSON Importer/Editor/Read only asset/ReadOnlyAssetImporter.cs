using System;
using QuickEye.ImportableAssets.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace QuickEye.ImportableAssets.Samples.JsonImporter.Editor
{
    /// <summary>
    /// Example of a readonly asset (Editor doesn't allow for edits) <see cref="ReadOnlyAssetImporterEditor"/>
    /// </summary>
    [ScriptedImporter(1, FileExtension)]
    public class ReadOnlyAssetImporter : ScriptableObjectImporter
    {
        private const string FileExtension = ".readonly-asset-example";

        [MenuItem("Assets/Create/Read Only Asset Example", false)]
        public static void CreateJsonAsset()
        {
            CreateTextAssetAction.StartFileNameEdit($"New asset{FileExtension}", "{ }");
        }

        public override Type ScriptableObjectType => typeof(ServerConfig);
        public override ISerializer Serializer => new JsonSerializer();
    }
}
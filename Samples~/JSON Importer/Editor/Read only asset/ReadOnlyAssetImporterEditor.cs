using UnityEditor;
using UnityEditor.AssetImporters;

namespace QuickEye.ImportableAssets.Samples.JsonImporter.Editor
{
    [CustomEditor(typeof(ReadOnlyAssetImporter))]
    public class ReadOnlyAssetImporterEditor : ScriptedImporterEditor
    {
        protected override bool needsApplyRevert => false;
        public override void OnInspectorGUI()
        {
        }
    }
}
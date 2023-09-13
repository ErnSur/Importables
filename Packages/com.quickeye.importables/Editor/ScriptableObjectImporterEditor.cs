using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor
{
    [PublicAPI]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObjectImporter), true)]
    public class ScriptableObjectImporterEditor : ScriptedImporterEditor
    {
        [SerializeField]
        private Font monospaceFont;

        // keep preview instance here instead of using `CustomPreviewAttribute` because the attribute doesn't work on child classes- descendants of this editor wouldn't get the preview
        [SerializeField]
        private TextFilePreview textPreview = new TextFilePreview();

        private bool _missingImporterSettings;

        private readonly EditableAssetTargetView _assetEditor = new EditableAssetTargetView();

        protected override bool needsApplyRevert => true;
        public override bool showImportedObject => false;

        public override void OnEnable()
        {
            base.OnEnable();
            SetUp();
        }

        public override void OnDisable()
        {
            base.OnDisable();
#if UNITY_2021_1_OR_NEWER
            textPreview.Cleanup();
#endif
        }

        private void SetUp()
        {
            foreach (var importer in targets.OfType<ScriptableObjectImporter>())
            {
                if (!importer.CanImportAsset())
                    _missingImporterSettings = true;
            }

            textPreview.font = monospaceFont;
            textPreview.Initialize(targets);
            if (_missingImporterSettings)
                return;
            _assetEditor?.SetUp(assetTargets);
        }

        protected override void OnHeaderGUI()
        {
            if (serializedObject.hasModifiedProperties)
                serializedObject.ApplyModifiedProperties();
            if (_assetEditor.IsInitialized)
                _assetEditor.OnHeaderGUI();
            else
                base.OnHeaderGUI();
        }

        public override void OnInspectorGUI()
        {
            if (_missingImporterSettings)
            {
                base.OnInspectorGUI();
                return;
            }

            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"),
                    new GUIContent("Importer Script"));
            _assetEditor.OnInspectorGUI();
            ApplyRevertGUI();
        }

        protected override void Apply()
        {
            if (_missingImporterSettings)
            {
                base.Apply();
                SetUp();
                return;
            }

            base.Apply();
            WriteTempObjectToFile();
            SetUp();
        }

        private void WriteTempObjectToFile()
        {
            var importers = targets.OfType<ScriptableObjectImporter>().ToArray();
            for (var i = 0; i < importers.Length; i++)
            {
                var importer = importers[i];
                var targetClone = _assetEditor.TargetClones[i];
                var newFileContent = importer.Serializer.ToBytes(targetClone);
                var path = AssetDatabase.GetAssetPath(importer);
                File.WriteAllBytes(path, newFileContent);
                AssetDatabase.ImportAsset(path);
            }

            _assetEditor.SerializedObjectModified = false;
        }

        protected override void ResetValues()
        {
            base.ResetValues();
            SetUp();
        }

        private void OnDestroy() => _assetEditor.OnDestroy();

        public override bool HasModified() =>
            _missingImporterSettings ? base.HasModified() : _assetEditor.SerializedObjectModified;

        #region IPreviewable

        public override bool HasPreviewGUI() => textPreview.HasPreviewGUI();
        public override string GetInfoString() => textPreview.GetInfoString();
        public override GUIContent GetPreviewTitle() => textPreview.GetPreviewTitle();
        public override void OnPreviewSettings() => textPreview.OnPreviewSettings();
        public override void OnPreviewGUI(Rect r, GUIStyle background) => textPreview.OnPreviewGUI(r, background);

        public override void OnInteractivePreviewGUI(Rect previewArea, GUIStyle background) =>
            textPreview.OnInteractivePreviewGUI(previewArea, background);

        public override void DrawPreview(Rect previewArea) => textPreview.DrawPreview(previewArea);

        public override void ReloadPreviewInstances() => textPreview.ReloadPreviewInstances();

        #endregion
    }
}
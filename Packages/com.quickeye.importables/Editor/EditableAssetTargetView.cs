using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor
{
    internal class EditableAssetTargetView
    {
        private UnityEditor.Editor _editor;
        public Object[] TargetClones { get; private set; }
        public bool SerializedObjectModified { get; set; }
        public bool IsInitialized => _editor != null;

        public void SetUp(Object[] assetTargets)
        {
            if (assetTargets == null)
                return;
            SerializedObjectModified = false;
            TargetClones = assetTargets
                .Where(a => a != null)
                .Select(a =>
                {
                    var obj = Object.Instantiate(a);
                    obj.name = a.name;
                    return obj;
                }).ToArray();
            _editor = UnityEditor.Editor.CreateEditor(TargetClones);
        }

        public void OnHeaderGUI()
        {
            if (_editor != null)
                _editor.DrawHeader();
        }

        public void OnInspectorGUI()
        {
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                if (_editor != null)
                {
                    _editor.OnInspectorGUI();
                }

                if (changeCheck.changed)
                {
                    SerializedObjectModified = true;
                }
            }
        }

        public void OnDestroy()
        {
            Object.DestroyImmediate(_editor);
            foreach (var tempSo in TargetClones)
            {
                Object.DestroyImmediate(tempSo);
            }
        }
    }
}
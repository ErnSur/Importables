using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor
{
    [Serializable]
    public class TextFilePreview : ObjectPreview
    {
        public Font font;
        private Dictionary<Object, string> _fileContents = new Dictionary<Object, string>();

        public int fontSize = 13;

        [SerializeField]
        private Vector2 previewScrollPos;

        public override void Initialize(Object[] targets)
        {
            
            base.Initialize(targets);
            if (targets == null || targets.Length == 0)
                return;
            UpdateCache();
        }

        private void UpdateCache([CallerMemberName] string memname = "")
        {
            if (m_Targets == null || m_Targets.Length == 0)
                return;
            try
            {
                _fileContents.Clear();
                foreach (var previewTarget in m_Targets)
                {
                    var path = AssetDatabase.GetAssetPath(previewTarget);
                    var fileContent = File.ReadAllText(path);
                    _fileContents[previewTarget] = fileContent;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override bool HasPreviewGUI() => true;

        public override GUIContent GetPreviewTitle() => new GUIContent("File content");

        public override void OnPreviewSettings()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var style = new GUIStyle(EditorStyles.toolbarButton);
                style.fontStyle = FontStyle.Bold;
                if (GUILayout.RepeatButton("+", style))
                {
                    fontSize = Mathf.Min(fontSize + 1, 50);
                }

                if (GUILayout.RepeatButton("-", style))
                {
                    fontSize = Mathf.Max(fontSize - 1, 1);
                }
            }
        }

        public override void OnInteractivePreviewGUI(Rect previewArea, GUIStyle background)
        {
            if (!_fileContents.TryGetValue(target, out var fileContent))
                return;
            if (Event.current.type == EventType.Repaint)
                background.Draw(previewArea, false, false, false, false);
            try
            {
                var labelStyle = new GUIStyle(EditorStyles.largeLabel);
                labelStyle.fontSize = fontSize;
                labelStyle.font = font;
                var contentSize = labelStyle.CalcSize(new GUIContent(fileContent));
                var viewRect = new Rect(Vector2.zero, contentSize);
                previewScrollPos = GUI.BeginScrollView(previewArea, previewScrollPos, viewRect);
                EditorGUI.SelectableLabel(viewRect, fileContent, labelStyle);
                GUI.EndScrollView();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
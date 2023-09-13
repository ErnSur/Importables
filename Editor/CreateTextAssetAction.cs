using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor
{
    [PublicAPI]
    public class CreateTextAssetAction : EndNameEditAction
    {
        private string _fileContent;

        /// <summary>
        /// Starts file name editing in the project browser
        /// </summary>
        /// <param name="pathName">File name with extension</param>
        /// <param name="fileContent">Text that will be written to the file</param>
        /// <param name="iconName">Name of the icon that is displayed next to the text input field</param>
        [PublicAPI]
        public static void StartFileNameEdit(string pathName, string fileContent, string iconName = "TextAsset Icon")
        {
            var icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;
            var editAction = CreateInstance<CreateTextAssetAction>();
            editAction._fileContent = fileContent;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, editAction, pathName, icon, null);
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            File.WriteAllText(pathName, _fileContent);
            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }
}
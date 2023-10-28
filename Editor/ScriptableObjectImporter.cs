using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor
{
    /// <summary>
    /// Can be derived from to use with user file extension
    /// </summary>
    public abstract class ScriptableObjectImporter : ScriptedImporter
    {
        public abstract Type ScriptableObjectType { get; }
        public abstract ISerializer Serializer { get; }
        public virtual bool CanImportAsset() => ScriptableObjectType != null && Serializer != null;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (!CanImportAsset())
                return;
            var fileContent = File.ReadAllBytes(ctx.assetPath);
            Object so;
            try
            {
                so = Serializer.FromBytes(fileContent, ScriptableObjectType);
            }
            catch (Exception e)
            {
                Debug.LogError($"Asset import failed.");
                Debug.LogException(e);
                so = ScriptableObject.CreateInstance(ScriptableObjectType);
            }
            ctx.AddObjectToAsset("main", so);
            ctx.SetMainObject(so);
        }
    }
}
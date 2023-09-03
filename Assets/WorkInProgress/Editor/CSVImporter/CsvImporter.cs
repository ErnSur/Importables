using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor.WIP.CSVImporter
{
    [ScriptedImporter(1, ".csv-asset")]
    public class CsvImporter : ScriptedImporter
    {
        private Type _assetType;
        private readonly CsvSerializer _serializer = new CsvSerializer();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var allText = File.ReadAllText(ctx.assetPath);

            // TODO: Learn how streams work, file stream, read/write streams. How other serializers use them?
            ObjectCollection collection;
            try
            {
                collection = (ObjectCollection)_serializer.FromText(allText, _assetType);
            }
            catch (Exception e)
            {
                Debug.LogError($"Asset import failed.");
                Debug.LogException(e);
                collection = ScriptableObject.CreateInstance<ObjectCollection>();
            }

            ctx.AddObjectToAsset("main", collection);
            ctx.SetMainObject(collection);

            for (var i = 0; i < collection.items.Length; i++)
            {
                var item = collection.items[i];
                // TODO: use csv ID column for id
                ctx.AddObjectToAsset($"item {i}", item);
            }
        }
    }
}
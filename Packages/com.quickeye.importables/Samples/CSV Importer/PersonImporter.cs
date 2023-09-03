using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace QuickEye.ImportableAssets.Samples.CsvImporter
{
    [ScriptedImporter(1, ".people")]
    public class PersonImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var fileLines = File.ReadLines(ctx.assetPath)
                // Skip header
                .Skip(1);

            var collection = ScriptableObject.CreateInstance<PersonCollection>();

            var items = new List<Person>();

            foreach (var csvLine in fileLines)
            {
                try
                {
                    var so = GetPersonFromCsvRecord(csvLine);
                    so.name = so.firstName;
                    items.Add(so);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Asset import error: {e}");
                }
            }

            collection.people = items.ToArray();

            ctx.AddObjectToAsset("main", collection);
            ctx.SetMainObject(collection);

            for (var i = 0; i < collection.people.Length; i++)
            {
                var item = collection.people[i];
                ctx.AddObjectToAsset($"item {item.id}", item);
            }
        }

        private static Person GetPersonFromCsvRecord(string text)
        {
            var fields = text.Split(',');
            var obj = ScriptableObject.CreateInstance<Person>();
            int.TryParse(fields[0], out obj.id);
            obj.firstName = fields[1];
            int.TryParse(fields[2], out obj.age);
            return obj;
        }
    }
}
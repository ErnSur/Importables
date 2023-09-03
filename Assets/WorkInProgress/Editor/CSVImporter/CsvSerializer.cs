using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor.WIP.CSVImporter
{
    internal class CsvSerializer : TextSerializer
    {
        public override Object FromText(string text, Type objectType)
        {
            var collection = ScriptableObject.CreateInstance<ObjectCollection>();
            
            var lines = text.Split('\n');
            collection.items = new Object[lines.Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var item = FromTextCollectionItem(line, objectType);
                collection.items[i] = item;
            }

            return collection;
        }

        public override string ToText(Object obj)
        {
            throw new NotImplementedException();
        }

        private Object FromTextCollectionItem(string csvLine, Type objectType)
        {
            var obj = ScriptableObject.CreateInstance(objectType);
            //TODO: populate obj
            return obj;
        }
    }
}
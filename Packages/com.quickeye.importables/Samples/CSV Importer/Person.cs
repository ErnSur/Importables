using System;
using UnityEngine;

namespace QuickEye.ImportableAssets.Samples.CsvImporter
{
    [Serializable]
    public class Person : ScriptableObject
    {
        public int id;
        public string firstName;
        public int age;
    }
}

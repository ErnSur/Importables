using UnityEngine;

namespace QuickEye.ImportableAssets.Samples.CsvImporter
{
    [CreateAssetMenu]
    public class PersonCollection : ScriptableObject
    {
        public Person[] people;
    }
}
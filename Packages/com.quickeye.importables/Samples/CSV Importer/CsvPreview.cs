using QuickEye.ImportableAssets.Editor;
using UnityEditor;

namespace QuickEye.ImportableAssets.Samples.CsvImporter
{
    [CustomPreview(typeof(PersonImporter))]
    public class CsvPreview : TextFilePreview
    {
    }
}
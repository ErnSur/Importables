using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor.Tests
{
    public class TestObj
    {
        public Object Field;
        public Object Field2;
        public TestObj Nested;
        public Object[] Array;
        public List<Object> List = new List<Object>();
        public Dictionary<string, Object> Dic = new Dictionary<string, Object>();
    }
}
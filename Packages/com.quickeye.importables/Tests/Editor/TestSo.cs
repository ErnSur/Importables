using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor.Tests
{
    [CreateAssetMenu]
    public class TestSo : ScriptableObject
    {
        public Object obj1;
        public Object obj2;
        public Object obj3;
        public List<Object> objList = new List<Object>();
        public Object[] objArray;
        public Dictionary<string,Object> objDic = new Dictionary<string, Object>();

        public void Reset()
        {
            obj1 = null;
            obj2 = null;
            obj3 = null;
            objList = new List<Object>();
            objArray = Array.Empty<Object>();
            objDic = new Dictionary<string, Object>();
        }
    }
}
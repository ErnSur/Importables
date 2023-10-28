using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor.Tests
{
    public class JsonConverterTests
    {
        private static IEnumerable<JsonConverter> UnityObjectConverters
        {
            get
            {
                yield return new RuntimeUnityObjectConverter();
                yield return new EditorUnityObjectConverter();
            }
        }

        private ScriptableObject _asset;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Packages/com.quickeye.importables/Tests/Editor/Assets/TestSo.asset");
        }

        [TestCaseSource(nameof(UnityObjectConverters))]
        public void Should_DeserializeUnityObjects(JsonConverter unityObjectJsonConverter)
        {
            var expected = new TestObj
            {
                Field = _asset,
                Field2 = _asset,
                Nested = new TestObj()
                {
                    Field = _asset
                },
                Array = new Object[]
                {
                    _asset,
                    _asset
                },
                List = new List<Object>
                {
                    _asset,
                    _asset
                },
                Dic = new Dictionary<string, Object>()
                {
                    { "1", _asset },
                    { "2", _asset },
                }
            };
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(unityObjectJsonConverter);
            settings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(expected, settings);
            var newObj = JsonConvert.DeserializeObject<TestObj>(json, settings);

            Assert.AreEqual(expected.Field, newObj.Field, nameof(TestObj.Field));
            Assert.AreEqual(expected.Field2, newObj.Field2, nameof(TestObj.Field2));
            Assert.AreEqual(expected.Nested.Field, newObj.Nested.Field, nameof(TestObj.Nested));
            CollectionAssert.AreEquivalent(expected.Array, newObj.Array, nameof(TestObj.Array));
            CollectionAssert.AreEquivalent(expected.List, newObj.List, nameof(TestObj.List));
            CollectionAssert.AreEquivalent(expected.Dic, newObj.Dic, nameof(TestObj.Dic));
        }
    }
}
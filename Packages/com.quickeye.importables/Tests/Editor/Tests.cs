using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ImportableAssets.Editor.Tests
{
    [TestOf(typeof(EditorJsonSerializer))]
    public class EditorJsonSerializerTests
    {
        private TestSo _testSo1;
        private TestSo _testSo2;
        private TestSo _testSo3;
        private EditorJsonSerializer _serializer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testSo1 = AssetDatabase.LoadAssetAtPath<TestSo>(
                "Packages/com.quickeye.importables/Tests/Editor/Assets/TestSo 1.asset");
            _testSo2 = AssetDatabase.LoadAssetAtPath<TestSo>(
                "Packages/com.quickeye.importables/Tests/Editor/Assets/TestSo 2.asset");
            _testSo3 = AssetDatabase.LoadAssetAtPath<TestSo>(
                "Packages/com.quickeye.importables/Tests/Editor/Assets/TestSo 3.asset");
        }

        [SetUp]
        public void SetUp()
        {
            _serializer = new EditorJsonSerializer();
            _testSo1.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            _testSo1.Reset();
        }

        [Test]
        public void Should_SerializeUnityObjects_When_ClassHasMultipleObjectFields()
        {
            _testSo1.obj2 = _testSo2;
            _testSo1.obj3 = _testSo3;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_testSo2, out var guid2, out long id2);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(_testSo3, out var guid3, out long id3);

            var json = _serializer.ToText(_testSo1);

            StringAssert.Contains(guid2, json);
            StringAssert.Contains(guid3, json);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_ClassHasMultipleObjectFields()
        {
            _testSo1.obj2 = _testSo2;
            _testSo1.obj3 = _testSo3;
            var json = _serializer.ToText(_testSo1);

            Debug.Log(json);
            var newObj = (TestSo)_serializer.FromText(json, _testSo1.GetType());

            Assert.AreEqual(_testSo1.obj1, newObj.obj1);
            Assert.AreEqual(_testSo1.obj2, newObj.obj2);
            Assert.AreEqual(_testSo1.obj3, newObj.obj3);
            Object.DestroyImmediate(newObj);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_ProcessingLists()
        {
            var expected = new List<Object>
            {
                _testSo2,
                _testSo3
            };
            _testSo1.objList = expected;
            var json = _serializer.ToText(_testSo1);

            Debug.Log(json);
            var newObj = (TestSo)_serializer.FromText(json, _testSo1.GetType());

            CollectionAssert.AreEquivalent(expected, newObj.objList);
            Object.DestroyImmediate(newObj);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_ProcessingArray()
        {
            var expected = new Object[]
            {
                _testSo2,
                _testSo3
            };
            _testSo1.objArray = expected;
            var json = _serializer.ToText(_testSo1);

            Debug.Log(json);
            var newObj = (TestSo)_serializer.FromText(json, _testSo1.GetType());

            CollectionAssert.AreEquivalent(expected, newObj.objArray);
            Object.DestroyImmediate(newObj);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_ProcessingDictionary()
        {
            var expected = new Dictionary<string, Object>()
            {
                { "1", _testSo2 },
                { "2", _testSo3 }
            };
            _testSo1.objDic = expected;
            var json = _serializer.ToText(_testSo1);

            Debug.Log(json);
            var newObj = (TestSo)_serializer.FromText(json, _testSo1.GetType());

            CollectionAssert.AreEquivalent(expected, newObj.objDic);
            Object.DestroyImmediate(newObj);
        }
    }
}
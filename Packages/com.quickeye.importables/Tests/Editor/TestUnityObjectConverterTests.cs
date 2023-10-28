using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets.Editor.Tests
{
    public class TestUnityObjectConverterTests
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
        public void Should_DeserializeUnityObjects_When_PocoHasUnityObjectFields()
        {
            var target = new TestObj();
            target.Field = _testSo2;
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EditorUnityObjectConverter());
            settings.Formatting = Formatting.Indented;
            
            var json = JsonConvert.SerializeObject(target, settings);
            Debug.Log(json);
            var newObj = JsonConvert.DeserializeObject<TestObj>(json, settings);

            Assert.AreEqual(target.Field, newObj.Field);
        }
        
        [Test]
        public void Should_DeserializeUnityObjects_When_PocoProcessingLists()
        {
            var expected = new List<Object>
            {
                _testSo2,
                _testSo3
            };
            var target = new TestObj
            {
                List = expected
            };
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EditorUnityObjectConverter());
            settings.Formatting = Formatting.Indented;
            
            var json = JsonConvert.SerializeObject(target, settings);
            Debug.Log(json);
            var newObj = JsonConvert.DeserializeObject<TestObj>(json, settings);

            CollectionAssert.AreEquivalent(expected, newObj.List);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_PocoProcessingArray()
        {
            var expected = new Object[]
            {
                _testSo2,
                _testSo3
            };
            var target = new TestObj
            {
                Array = expected
            };
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EditorUnityObjectConverter());
            settings.Formatting = Formatting.Indented;
            
            var json = JsonConvert.SerializeObject(target, settings);
            Debug.Log(json);
            var newObj = JsonConvert.DeserializeObject<TestObj>(json, settings);

            CollectionAssert.AreEquivalent(expected, newObj.Array);
        }

        [Test]
        public void Should_DeserializeUnityObjects_When_PocoProcessingDictionary()
        {
            var expected = new Dictionary<string, Object>()
            {
                { "1", _testSo2 },
                { "2", _testSo3 }
            };
            var target = new TestObj
            {
                Dic = expected
            };
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new EditorUnityObjectConverter());
            settings.Formatting = Formatting.Indented;
            
            var json = JsonConvert.SerializeObject(target, settings);
            Debug.Log(json);
            var newObj = JsonConvert.DeserializeObject<TestObj>(json, settings);

            CollectionAssert.AreEquivalent(expected, newObj.Dic);
        }
    }
}
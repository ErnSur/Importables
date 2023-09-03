using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    public class PropertyWrapperScope : IDisposable
    {
        private readonly Action<Rect, SerializedProperty> _begin, _end;
        private SerializedProperty _lastProperty;
        private Rect _lastRect;
        
        public PropertyWrapperScope(Action<Rect, SerializedProperty> begin, Action<Rect, SerializedProperty> end)
        {
            _begin = begin;
            _end = end;
        }

        public void Dispose()
        {
        }
    }
}
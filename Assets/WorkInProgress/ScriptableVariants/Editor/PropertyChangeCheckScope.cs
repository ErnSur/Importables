using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    // lets assume that after modyfying any prop at the end of onInspectorGUI GYU.changed is trigered again
    public class PropertyChangeCheckScope : IDisposable
    {
        private readonly Action<SerializedProperty> _onPropertyChange;
        private SerializedProperty _lastProperty;

        public PropertyChangeCheckScope(Action<SerializedProperty> onPropertyChange)
        {
            _onPropertyChange = onPropertyChange;
        }

        public void Dispose()
        {
            _lastProperty?.Dispose();
        }
    }
}
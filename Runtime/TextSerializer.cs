using System;
using System.Text;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    public abstract class TextSerializer : ISerializer
    {
        public abstract Object FromText(string text, Type objectType);
        public abstract string ToText(Object obj);

        public byte[] ToBytes(Object obj)
        {
            return Encoding.UTF8.GetBytes(ToText(obj));
        }

        public Object FromBytes(byte[] data, Type objectType)
        {
            return FromText(Encoding.UTF8.GetString(data), objectType);
        }
    }
}
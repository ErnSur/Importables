using System;
using Object = UnityEngine.Object;

namespace QuickEye.ImportableAssets
{
    public interface ISerializer
    {
        Object FromBytes(byte[] data, Type objectType);
        byte[] ToBytes(Object obj);
    }
}
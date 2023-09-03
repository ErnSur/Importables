// using System;
// using Newtonsoft.Json;
//
// namespace QuickEye.AssetImporterExtensions
// {
//     public class AssetReferenceJsonConverter : JsonConverter
//     {
//
//         public AssetReferenceJsonConverter() : base()
//         {
//
//         }
//
//         public override bool CanConvert(Type objectType)
//         {
//             return typeof(AssetReference).IsAssignableFrom(objectType);
//         }
//
//         public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//         {
//
//             string assetGuid = (string)reader.Value;
//             AssetReference assetRef = new AssetReference(assetGuid);
//
//             return assetRef;
//
//         }
//
//         public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//         {
//             var assetRef = value as AssetReference;
//             writer.WriteValue(assetRef.AssetGUID);
//         }
//     }
// }
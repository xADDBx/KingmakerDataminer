using Kingmaker.Blueprints;
using Kingmaker.Blueprints.DirectSerialization;
using Newtonsoft.Json;
using System;

namespace CustomBlueprints
{
    /*
    TODO:
    ChapterList gets treated as a BlueprintScriptableObject but gives in a BlueprintReference
    public IEnumerator<BlueprintEncyclopediaChapter> GetEnumerator()
    {
        return (from r in this.m_List
        select (BlueprintEncyclopediaChapter)r.Get()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.m_List.GetEnumerator();
    }
    */
    public class BlueprintAssetIdConverter : JsonConverter
    {
        public BlueprintAssetIdConverter() { }

        public override void WriteJson(JsonWriter w, object o, JsonSerializer szr)
        {
            try
            {
                BlueprintScriptableObject bp = null;
                if (o is BlueprintReference bpRef)
                {
                    bp = bpRef.Get();
                } else
                {
                    bp = (BlueprintScriptableObject)o;
                }
                w.WriteStartObject();
                w.WritePropertyName("Type");
                w.WriteValue("Blueprint");
                w.WritePropertyName("AssetId");
                w.WriteValue("!bp_" + bp.AssetGuid);
                w.WritePropertyName("Name");
                w.WriteValue(bp.name);
                w.WriteEndObject();
            }
            catch (InvalidCastException ex)
            {
                w.WriteStartObject();
                w.WritePropertyName("Type");
                w.WriteValue("Blueprint");
                w.WritePropertyName("BlueprintType");
                w.WriteValue(o?.GetType().FullName);
                w.WritePropertyName("Name");
                w.WriteValue(ex.ToString());
                w.WriteEndObject();
            }
        }
        public override object ReadJson(
          JsonReader reader,
          Type objectType,
          object existingValue,
          JsonSerializer serializer
        )
        {
            throw new NotImplementedException();
        }
        private static readonly Type _tBlueprintScriptableObject = typeof(BlueprintScriptableObject);
        public override bool CanConvert(Type type) => _tBlueprintScriptableObject.IsAssignableFrom(type);
    }
}
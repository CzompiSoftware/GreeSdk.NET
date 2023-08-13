using GreeSdk.Packs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GreeSdk.Serialization;

public class JsonPackConverter : JsonConverter<Pack>
{
    public override Pack Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Pack value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

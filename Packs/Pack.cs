using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class Pack
{
    [JsonPropertyName("t")]
    public string Type { get; protected set; }

    [JsonPropertyName("mac")]
    public string Mac { get; set; }
}

using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class StatusPack : Pack
{
    [JsonPropertyName("cols")]
    public string[] Options { get; set; }

    public StatusPack()
    {
        Type = "status";
    }
}

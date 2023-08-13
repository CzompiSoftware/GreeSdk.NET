using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class BindOkPack : Pack
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("r")]
    public int ResultCode { get; set; }

    public BindOkPack()
    {
        Type = "bindok";
    }
}

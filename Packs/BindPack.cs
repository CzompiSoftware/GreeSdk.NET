using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class BindPack : Pack
{
    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    public BindPack()
    {
        Type = "bind";
    }
}

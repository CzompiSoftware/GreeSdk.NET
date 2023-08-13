using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class ResultPack : Pack
{
    [JsonPropertyName("r")]
    public int ResultCode { get; set; }

    [JsonPropertyName("opt")]
    public string[] Options { get; set; }

    [JsonPropertyName("p")]
    public int[] Values { get; set; }

    public ResultPack()
    {
        Type = "res";
    }
}

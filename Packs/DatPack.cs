using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class DatPack : Pack
{

    [JsonPropertyName("r")]
    public int ResultCode { get; set; }

    [JsonPropertyName("cols")]
    public string[] Options { get; set; }

    [JsonPropertyName("dat")]
    public int[] Values { get; set; }

    public DatPack()
    {
        Type = "dat";
    }
}

using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class DevicePack : Pack
{
    [JsonPropertyName("cid")]
    public string ClientId { get; set; }

    [JsonPropertyName("bc")]
    public string BrandCode { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }

    [JsonPropertyName("catalog")]
    public string Catalog { get; set; }

    [JsonPropertyName("mid")]
    public string ModelId { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("series")]
    public string Series { get; set; }

    [JsonPropertyName("ver")]
    public string FirmwareVersion { get; set; }

    [JsonPropertyName("lock")]
    public int Lock { get; set; }

    [JsonPropertyName("vender")]
    public string Vendor { get; set; }

    public DevicePack()
    {
        Type = "dev";
    }
}

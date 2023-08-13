using GreeSdk.Packs;
using System.Text.Json.Serialization;

namespace GreeSdk.Packets;

public class Packet
{

    [JsonPropertyName("t")]
    public string Type { get; protected set; }

    [JsonPropertyName("i")]
    public int I { get; set; }
    
    [JsonPropertyName("uid")]
    public int Uid { get; set; }

    [JsonPropertyName("cid")]
    public string ClientId { get; set; }

    [JsonPropertyName("tcid")]
    public string TargetClientId { get; set; }

    [JsonPropertyName("pack")]
    public string EncryptedPack { get; set; }

    [JsonIgnore]
    public virtual Pack Pack { get; internal set; }
}


using System.Text.Json.Serialization;

namespace GreeSdk.Packs;

public class CommandPack : Pack
{
    [JsonPropertyName("opt")]
    public string[] Options { get; set; }

    [JsonPropertyName("p")]
    public int[] Values { get; set; }

    public CommandPack()
    {
        Type = "cmd";
    }
}

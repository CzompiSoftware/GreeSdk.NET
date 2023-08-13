using System.Text.Json.Serialization;

namespace GreeSdk.Packets;

public class WifiSettingsPacket : Packet
{
    [JsonPropertyName("ssid")]
    public string Ssid;
 
    [JsonPropertyName("psw")]
    public string Password;

    public WifiSettingsPacket()
    {
        Type = "wlan";
    }
}


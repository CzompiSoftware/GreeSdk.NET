using CzomPack.Logging;
using GreeSdk.Network;
using GreeSdk.Packets;
using GreeSdk.Packs;
using System.Text.Json;

namespace GreeSdk;

public class Utils {

    public static Dictionary<String, int> Zip(string[] keys, int[] values) {
        if (keys.Length != values.Length)
            throw new ArgumentException("Length of keys and values must match");

        Dictionary<String, int> zipped = new();
        for (int i = 0; i < keys.Length; i++) {
            zipped.Add(keys[i], values[i]);
        }

        return zipped;
    }

    public static Dictionary<string, int> getValues(DatPack pack) {
        return Zip(pack.Options, pack.Values);
    }

    public static string SerializePacket(Packet packet, DeviceKeyChain deviceKeyChain) {
        
        if (packet.Pack != null) {
            string key = GetKey(deviceKeyChain, packet);
            string plainPack = JsonSerializer.Serialize(packet.Pack);
            packet.EncryptedPack = Crypto.Encrypt(plainPack, key);
        }

        return JsonSerializer.Serialize(packet);
    }

    public static Packet DeserializePacket(String jsonString, DeviceKeyChain deviceKeyChain) {
        Packet packet = JsonSerializer.Deserialize<Packet>(jsonString);

        packet.Pack = packet.GetPack(deviceKeyChain);

        return packet;
    }

    internal static string GetKey(DeviceKeyChain keyChain, Packet packet) {
        string key = Crypto._genericKey;

        Logger.Info<Utils>(string.Format("cid: %s, tcid: %s", packet.ClientId, packet.TargetClientId));

        if (keyChain != null) {
            if (keyChain.ContainsKey(packet.ClientId)) {
                key = keyChain.GetKey(packet.ClientId);
            } else if (keyChain.ContainsKey(packet.TargetClientId)) {
                key = keyChain.GetKey(packet.TargetClientId);
            }
        }

        return key;
    }
}
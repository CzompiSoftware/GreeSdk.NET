using GreeSdk.Devices;
using GreeSdk.Network;
using GreeSdk.Packets;
using GreeSdk.Packs;
using System.Text.Json;

namespace GreeSdk;

internal static class Extensions
{
    internal static Pack GetPack(this Packet packet, DeviceKeyChain deviceKeyChain)
    {
        if (packet.EncryptedPack != null)
        {
            String key = Utils.GetKey(deviceKeyChain, packet);
            String plainPack = Crypto.Decrypt(packet.EncryptedPack, key);
            return JsonSerializer.Deserialize<Pack>(plainPack);
        }

        return null;
    }

    internal static string GetDescriptor(this Parameter val)
    {
        ParameterDescriptorAttribute[]? attributes = (ParameterDescriptorAttribute[]?)val.GetType().GetField(val.ToString())?.GetCustomAttributes(typeof(ParameterDescriptorAttribute), false);
        return attributes != null && attributes.Length > 0 ? attributes.First().Descriptor : string.Empty;
    }
}
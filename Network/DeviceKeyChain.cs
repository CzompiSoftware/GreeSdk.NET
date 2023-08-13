namespace GreeSdk.Network;

public class DeviceKeyChain
{
    private readonly Dictionary<string, string> _keys = new();

    public void AddKey(string deviceId, string key)
    {
        _keys.Add(deviceId.ToLowerInvariant(), key);
    }

    public string GetKey(string deviceId)
    {
        if (deviceId == null)
            return null;

        string id = deviceId.ToLowerInvariant();

        if (!_keys.ContainsKey(id))
            return null;

        return _keys[id];
    }

    public bool ContainsKey(string deviceId)
    {
        if (deviceId == null)
            return false;

        return _keys.ContainsKey(deviceId.ToLowerInvariant());
    }
}

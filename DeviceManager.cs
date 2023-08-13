using CzomPack.Logging;
using GreeSdk.Network;
using GreeSdk.Packets;
using GreeSdk.Packs;
using Serilog;
using System.Net.Sockets;
using System.Text.Json;
using System.Text;
using GreeSdk.Devices;

namespace GreeSdk;

public delegate void DeviceManagerEventHanlder();
public class DeviceManager
{
    private readonly string LOG_TAG = "DeviceManager";
    private readonly int DATAGRAM_PORT = 7000;

    private static DeviceManager instance = null;

    private readonly Dictionary<string, IAppliance> devices = new();
    private readonly DeviceKeyChain keyChain = new();
    //private readonly List<DeviceManagerEventListener> eventListeners = new();

    public event DeviceManagerEventHanlder OnDeviceListUpdated;
    public event DeviceManagerEventHanlder OnDeviceStatusUpdated;

    public static DeviceManager getInstance()
    {
        instance ??= new DeviceManager();

        return instance;
    }

    protected DeviceManager()
    {
        Logger.Info<DeviceManager>("Created");
    }

    public IAppliance[] GetDevices()
    {
        return devices.Values.ToArray();
    }

    public IAppliance GetDevice(string deviceId)
    {
        if (devices.ContainsKey(deviceId))
            return devices[deviceId];

        return null;
    }

    public void RegisterEventListener(DeviceManagerEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterEventListener(DeviceManagerEventListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void SetParameter(IAppliance device, string name, int value)
    {
        Dictionary<string, int> p = new()
        {
            { name, value }
        };

        SetParameters(device, p);
    }

    public void SetParameters(IAppliance device, Dictionary<string, int> parameters)
    {
        Logger.Debug<DeviceManager>(string.Format($"Setting parameters of {{id}}: {parameters}", device.Id));

        AppPacket packet = new()
        {
            TargetClientId = device.Id,
            I = 0
        };

        CommandPack pack = new()
        {
            Options = parameters.Keys.ToArray(),
            Values = parameters.Values.ToArray(),
            Mac = packet.TargetClientId
        };

        packet.Pack = pack;

    }

    public void SetWifi(string ssid, string password)
    {

        WifiSettingsPacket packet = new()
        {
            Ssid = ssid,
            Password = password
        };

        comm.execute(new WifiSettingsPacket[] { packet });
    }

    public void Discover()
    {
        Logger.Info<DeviceManager>("Device discovery running...");

        ScanPacket sp = new ScanPacket();

        comm.execute(new ScanPacket[] { sp });
    }

    public void UpdateDevices()
    {
        if (!devices.Any())
        {
            Logger.Info<DeviceManager>("No devices to update");
            return;
        }

        Logger.Info<DeviceManager>("Updating {deviceCount} device(s)", devices.Count);

        List<AppPacket> packets = new();

        List<string> keys = new();
        foreach (Parameter p in Enum.GetValues<Parameter>())
        {
            keys.Add(p.ToString());
        }

        foreach (IAppliance device in devices.Values)
        {
            AppPacket packet = new()
            {
                TargetClientId = device.Id,
                I = 0
            };

            StatusPack pack = new()
            {
                Options = keys.ToArray(),
                Mac = device.Id
            };
            packet.Pack = pack;

            packets.Add(packet);
        }

        var comm = new NetworkCommunicator(keyChain);

        comm.Execute(packets.ToArray());
    }

    private void BindDevices(Packet[] scanResponses)
    {
        List<AppPacket> requests = new();

        for (int i = 0; i < scanResponses.Length; i++)
        {
            Packet response = scanResponses[i];

            if (response.Pack is not DevicePack)
                continue;

            DevicePack devicePack = (DevicePack)response.Pack;

            IAppliance device;

            if (!devices.ContainsKey(response.ClientId))
            {
                device = new Appliance(devicePack.Mac, this);
                devices.Add(devicePack.Mac, device);
            }
            else
            {
                device = devices[response.ClientId];
            }
            device.UpdateWithDevicePack(devicePack);

            if (!keyChain.ContainsKey(response.ClientId))
            {
                Logger.Info<DeviceManager>("Binding device: " + devicePack.Name);

                AppPacket request = new()
                {
                    TargetClientId = response.ClientId
                };
                request.Pack = new()
                {
                    Mac = request.TargetClientId
                };
                request.I = 1;

                requests.Add(request);
            }
        }
    }

    private void StoreDevices(Packet[] bindResponses)
    {
        foreach (Packet response in bindResponses)
        {
            if (response.Pack is not BindOkPack)
                continue;

            BindOkPack pack = (BindOkPack)response.Pack;

            Logger.Info<DeviceManager>("Storing key for device: " + pack.Mac);
            keyChain.AddKey(pack.Mac, pack.Key);
        }

        SendEvent(DeviceManagerEventListener.Event.DEVICE_LIST_UPDATED);
        UpdateDevices();
    }

    private void SendEvent(DeviceManagerEventListener.Event @event)
    {
        foreach (DeviceManagerEventListener listener in eventListeners)
            listener.onEvent(@event);
    }

    /// <summary>
    /// Sends a request to the actual device and waits a few seconds for the response.
    /// </summary>
    /// <param name="request">Request object which encapsulates the encrypted pack</param>
    /// <returns>The response object which encapsulates the encrypted response pack</returns>
    /// <exception cref="IOException"/>
    private async Task<Packet> SendDeviceRequest(Packet request)
    {
        Logger.Debug<DeviceManager>("Sending device request");

        var datagram = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(request));
        Logger.Debug<DeviceManager>($"{{byteCount}} bytes will be sent", datagram.Length);

        using (var udp = new UdpClient())
        {
            var sent = await udp.SendAsync(datagram, datagram.Length, .Address, 7000);
            Logger.Debug<DeviceManager>($"{{byteCount}} bytes sent to {{address}}", sent, _model.Address);

            for (int i = 0; i < 20; ++i)
            {
                if (udp.Available > 0)
                {
                    var results = await udp.ReceiveAsync();
                    Logger.Debug<DeviceManager>($"Got response, {{byteCount}} bytes", results.Buffer.Length);

                    var json = Encoding.ASCII.GetString(results.Buffer);
                    var response = JsonSerializer.Deserialize<Packet>(json);

                    return response;
                }

                await Task.Delay(100);
            }

            Logger.Warning<DeviceManager>("Request timed out");

            throw new IOException("Device request timed out");
        }
    }
}

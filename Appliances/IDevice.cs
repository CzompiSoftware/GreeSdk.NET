namespace GreeSdk.Devices;

public interface IAppliance
{
    string Id { get; protected set; }

    string Name { get; protected set; }

    Mode Mode { get; set; }

    FanSpeed FanSpeed { get; set; }

    int Temperature { get; protected set; }

    /// <summary>
    /// Sets the temperature based on <paramref name="tempUnit"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="tempUnit"></param>
    void SetTemperature(int value, TemperatureUnit tempUnit);

    /// <summary>
    /// When powered on, it returns true, otherwise false.
    /// </summary>
    bool IsPoweredOn { get; set; }

    bool IsLight { get; set; }

    bool IsQuietMode { get; set; }

    bool IsTurboMode { get; set; }

    bool IsHealthMode { get; set; }

    bool IsAirMode { get; set; }

    bool IsXFanMode { get; set; }

    bool IsSavingMode { get; set; }

    bool IsSleep { get; }

    int SleepMode { get; }

    VerticalSwingMode VerticalSwingMode { get; set; }

    int GetParameter(string name);
    void SetParameter(string name, int value);

    void SetWifiDetails(string ssid, string password);
}

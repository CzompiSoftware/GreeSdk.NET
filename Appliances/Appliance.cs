using CzomPack.Logging;
using GreeSdk.Packs;
using Serilog;

namespace GreeSdk.Devices;

public class Appliance : IAppliance
{

    public Appliance(String deviceId, DeviceManager deviceManager)
    {
        this.Id = deviceId;
        this._deviceManager = deviceManager;
        this._logTag = string.Format("Device(%s)", deviceId);

        //Log.i(this.logTag, "Created");
    }

    public void UpdateWithDatPack(DatPack pack)
    {
        UpdateParameters(Utils.Zip(pack.Options, pack.Values));
    }

    public void UpdateWithResultPack(ResultPack pack)
    {
        UpdateParameters(Utils.Zip(pack.Options, pack.Values));
    }

    public void UpdateWithDevicePack(DevicePack pack)
    {
        Logger.Debug<Device>($"[{_logTag}]: Updating name: {{name}}", pack.Name);
        Name = pack.Name;
    }
    private readonly DeviceManager _deviceManager;
    private readonly string _logTag;

    private string _id;
    public string Id { get => _id; set => _id = value; }

    private string _name;
    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TemperatureUnit TemperatureUnit { get; set; }
    public int Temperature { get; set; }


    private Mode _mode;
    public Mode Mode { get => _mode; set { _mode = value; SetParameter(Parameter.MODE, (int)value); } }

    private FanSpeed _fanSpeed;
    public FanSpeed FanSpeed { get => _fanSpeed; set { _fanSpeed = value; SetParameter(Parameter.FAN_SPEED, (int)_fanSpeed); } }

    public bool _isPoweredOn;
    public bool IsPoweredOn { get => _isPoweredOn; set { _isPoweredOn = value; SetParameter(Parameter.POWER, _isPoweredOn ? 1 : 0); } }

    public bool _isPoweredOn;
    public bool IsLight { get => _isPoweredOn; set { _isPoweredOn = value; SetParameter(Parameter.LIGHT, _isPoweredOn ? 1 : 0); } }

    public bool _isPoweredOn;
    public bool IsQuietMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool _isPoweredOn;
    public bool IsTurboMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool _isPoweredOn;
    public bool IsHealthMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool _isPoweredOn;
    public bool IsAirMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool _isPoweredOn;
    public bool IsXFanMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool _isPoweredOn;
    public bool IsSavingMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsSleep { get; private set; }
    public int SleepMode { get; private set; }

    public VerticalSwingMode _verticalSwingMode = VerticalSwingMode.DEFAULT;
    public VerticalSwingMode VerticalSwingMode { get => _verticalSwingMode; set => throw new NotImplementedException(); }

    public void SetMode(Mode mode)
    {
        SetParameter(Parameter.MODE, (int)mode);
    }


    public FanSpeed getFanSpeed()
    {
        return fanSpeed;
    }


    public void setFanSpeed(FanSpeed fanSpeed)
    {
        setParameter(Parameter.FAN_SPEED, fanSpeed.ordinal());
    }

    public override void SetTemperature(int value, TemperatureUnit tempUnit)
    {
        Temperature = value;
        TemperatureUnit = tempUnit;
        SetParameters(
            new Parameter[] { Parameter.TEMPERATURE, Parameter.TEMPERATURE_UNIT },
            new int[] { value, (int)TemperatureUnit }
        );
    }


    public void SetSleepState(bool state)
    {
        SetParameters(new Parameter[] { Parameter.SLEEP, Parameter.SLEEP_MODE }, new int[] { state ? 1 : 0, state ? 1 : 0 });
    }


    public int GetParameter(string name)
    {
        return 0;
    }


    public void SetWifiDetails(String ssid, String password)
    {
        _deviceManager.SetWifi(ssid, password);
    }


    public override bool Equals(Object o)
    {
        if (this == o) return true;
        if (o == null || this != o) return false;

        Device device = (Device)o;

        return Id.Equals(device.Id);
    }


    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private void SetParameter(Parameter parameter, int value)
    {
        SetParameter(parameter.GetDescriptor(), value);
    }

    private void SetParameters(Parameter[] parameters, int[] values)
    {
        String[] names = new String[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            names[i] = parameters[i].GetDescriptor();
        }

        _deviceManager.SetParameters(this, Utils.Zip(names, values));
    }

    private void UpdateParameters(Dictionary<String, int> parameterMap)
    {
        Logger.Debug<Device>(_logTag, "Updating parameterMap: " + parameterMap);

        Mode = GetEnumParameter(parameterMap, Parameter.MODE, Mode);
        FanSpeed = GetEnumParameter(parameterMap, Parameter.FAN_SPEED, FanSpeed);
        Temperature = GetOrdinalParameter(parameterMap, Parameter.TEMPERATURE, Temperature);
        TemperatureUnit = GetEnumParameter(parameterMap, Parameter.TEMPERATURE_UNIT, TemperatureUnit);
        IsPoweredOn = GetBooleanParameter(parameterMap, Parameter.POWER, IsPoweredOn);
        IsLight = GetBooleanParameter(parameterMap, Parameter.LIGHT, IsLight);
        IsQuietMode = GetBooleanParameter(parameterMap, Parameter.QUIET_MODE, IsQuietMode);
        IsTurboMode = GetBooleanParameter(parameterMap, Parameter.TURBO_MODE, IsTurboMode);
        IsHealthMode = GetBooleanParameter(parameterMap, Parameter.HEALTH_MODE, IsHealthMode);
        IsAirMode = GetBooleanParameter(parameterMap, Parameter.AIR_MODE, IsAirMode);
        IsXFanMode = GetBooleanParameter(parameterMap, Parameter.XFAN_MODE, IsXFanMode);
        IsSavingMode = GetBooleanParameter(parameterMap, Parameter.SAVING_MODE, IsSavingMode);
        IsSleep = GetBooleanParameter(parameterMap, Parameter.SLEEP, IsSleep);
        SleepMode = GetOrdinalParameter(parameterMap, Parameter.SLEEP_MODE, SleepMode);
        VerticalSwingMode = GetEnumParameter(parameterMap, Parameter.VERTICAL_SWING, VerticalSwingMode);
    }

    private static T GetEnumParameter<T>(Dictionary<string, int> parameterMap, Parameter parameter, T def) where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        if (parameterMap.ContainsKey(parameter.GetDescriptor()))
        {
            int ordinal = parameterMap[parameter.GetDescriptor()];
            if (ordinal >= 0 && ordinal < values.Length)
            {
                return values[ordinal];
            }
        }

        return def;
    }

    private static int GetOrdinalParameter(Dictionary<String, int> parameterMap, Parameter parameter, int def)
    {
        if (parameterMap.ContainsKey(parameter.GetDescriptor()))
            return parameterMap[parameter.GetDescriptor()];
        return def;
    }

    private static bool GetBooleanParameter(Dictionary<String, int> parameterMap, Parameter parameter, bool def)
    {
        return GetOrdinalParameter(parameterMap, parameter, def ? 1 : 0) == 1;
    }



    public override string ToString()
    {
        return "Device{" +
                "deviceId='" + Id + '\'' +
                ", deviceManager=" + _deviceManager +
                ", logTag='" + _logTag + '\'' +
                ", name='" + Name + '\'' +
                ", mode=" + Mode +
                ", fanSpeed=" + FanSpeed +
                ", temperature=" + Temperature +
                ", temperatureUnit=" + TemperatureUnit +
                ", poweredOn=" + IsPoweredOn +
                ", lightEnabled=" + IsLight +
                ", quietModeEnabled=" + IsQuietMode +
                ", turboModeEnabled=" + IsTurboMode +
                ", healthModeEnabled=" + IsHealthMode +
                ", airModeEnabled=" + IsAirMode +
                ", xFanModeEnabled=" + IsXFanMode +
                ", savingModeEnabled=" + IsSavingMode +
                ", sleepEnabled=" + IsSleep +
                ", sleepMode=" + SleepMode +
                ", verticalSwingMode=" + VerticalSwingMode +
                '}';
    }
}

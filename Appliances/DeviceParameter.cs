namespace GreeSdk.Devices;

public enum Parameter
{
    [ParameterDescriptor("Pow")]
    POWER,

    [ParameterDescriptor("Mod")]
    MODE,
    
    [ParameterDescriptor("SetTem")]
    TEMPERATURE,
    
    [ParameterDescriptor("TemUn")]
    TEMPERATURE_UNIT,
    
    [ParameterDescriptor("WdSpd")]
    FAN_SPEED,
    
    [ParameterDescriptor("Air")]
    AIR_MODE,
    
    [ParameterDescriptor("Blo")]
    XFAN_MODE,
    
    [ParameterDescriptor("Health")]
    HEALTH_MODE,
    
    [ParameterDescriptor("SwhSlp")]
    SLEEP,
    
    [ParameterDescriptor("SlpMod")]
    SLEEP_MODE,
    
    [ParameterDescriptor("Quiet")]
    QUIET_MODE,
    
    [ParameterDescriptor("Tur")]
    TURBO_MODE,
    
    [ParameterDescriptor("SvSt")]
    SAVING_MODE,
    
    [ParameterDescriptor("Lig")]
    LIGHT,
    
    [ParameterDescriptor("SwingLfRig")]
    HORIZONTAL_SWING,
    
    [ParameterDescriptor("SwUpDn")]
    VERTICAL_SWING,
    
    [ParameterDescriptor("StHt")]
    STHT_MODE,
    
    [ParameterDescriptor("HeatCoolType")]
    HEAT_COOL_TYPE,
    
    [ParameterDescriptor("TemRec")]
    TEM_REC_MODE
}

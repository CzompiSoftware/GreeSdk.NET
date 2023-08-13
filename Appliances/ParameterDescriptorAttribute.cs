namespace GreeSdk.Devices;

internal class ParameterDescriptorAttribute : Attribute
{
    public ParameterDescriptorAttribute(string descriptor)
    {
        Descriptor = descriptor;
    }

    public string Descriptor { get; }
}
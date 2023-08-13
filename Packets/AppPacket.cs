namespace GreeSdk.Packets;

public class AppPacket: Packet {
    public AppPacket()
    {
        ClientId = "app";
        Type = "pack";
        Uid = 0;
    }
}


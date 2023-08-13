using CzomPack.Logging;
using GreeSdk;
using GreeSdk.Packets;
using Serilog;
using System.Net.Sockets;
using System;
using System.Text;
using System.Net;

namespace GreeSdk.Network;

internal class NetworkCommunicator : AsyncCommunicator<Packet, object, Packet>
{
    private readonly DeviceKeyChain _keyChain;
    private readonly int DATAGRAM_PORT = 7000;
    private readonly int TIMEOUT_MS = 500;
    private AsyncCommunicationFinishedListener communicationFinishedListener;
    private UdpClient socket;

    public NetworkCommunicator(DeviceKeyChain keyChain)
    {
        _keyChain = keyChain;
    }

    protected override void DoBeforeExecution()
    {
        throw new NotImplementedException();
    }

    protected override List<Packet> DoInBackground(params Packet[] args)
    {
        List<Packet> requests = new() { args[0] };
        List<Packet> responses = new();

        if (requests == null || requests.Count == 0)
            return responses;

        if (!CreateSocket())
            return responses;

        try
        {
            foreach (Packet request in requests)
            {
                BroadcastPacket(request);
            }
            responses = ReceivePackets(TIMEOUT_MS);
        }
        catch (Exception e)
        {
            Logger.Error<NetworkCommunicator>("Error: " + e.Message);
        }
        finally
        {
            CloseSocket();
        }

        return responses;
    }
    protected override void DoAfterExecution(params Packet[] args)
    {

        if (communicationFinishedListener != null)
            communicationFinishedListener.onFinished();
    }

    private void BroadcastPacket(Packet packet)
    {
        string data = Utils.SerializePacket(packet, _keyChain);

        Logger.Debug<NetworkCommunicator>("Broadcasting: " + data);

        socket.Send(Encoding.UTF8.GetBytes(data), data.Length, "255.255.255.255", DATAGRAM_PORT);
    }

    private Packet[] ReceivePackets(int timeout)
    {
        socket.setSoTimeout(timeout);

        List<Packet> responses = new();
        List<Socket> datagramPackets = new();

        try
        {
            while (true)
            {
                byte[] buffer = new byte[65536];
                System.Net.Sockets.Socket datagramPacket = new(new(SocketInformation(buffer, 65536));

                socket.Receive();

                datagramPackets.Add(datagramPacket);
            }
        }
        catch (Exception e)
        {
            Logger.Warning<NetworkCommunicator>("Exception: " + e.Message);
        }

        foreach (System.Net.Sockets.Socket p in datagramPackets)
        {
            string data = new string(p., 0, p.Length());
            InetAddress address = p.getAddress();

            Logger.Debug<NetworkCommunicator>(string.Format("Received response from %s: %s", address.getHostAddress(), data));

            Packet response = Utils.DeserializePacket(data, _keyChain);

            // Filter out packets sent by us
            if (response.ClientId != null && response.ClientId != "app")
                responses.Add(response);
        }

        return responses.ToArray();
    }

    private bool CreateSocket()
    {
        try
        {
            socket = new();
            socket.Connect("255.255.255.255", DATAGRAM_PORT);
        }
        catch (SocketException e)
        {
            Logger.Error<NetworkCommunicator>("Failed to create socket. Error: " + e.getMessage());
            return false;
        }

        return true;
    }

    private void CloseSocket()
    {
        Logger.Info<NetworkCommunicator>("Closing socket");

        socket.Close();
        socket = null;
    }

}
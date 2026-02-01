using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace JJK_Project_Server.Core.Network
{
  class GameServer : INetEventListener
  {

    private NetManager _netManager;

    public void Start(int port)
    {
      this._netManager = new NetManager(this);
      this._netManager.Start(port);

      Console.WriteLine("AI MEU CARALHO");
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
      request.AcceptIfKey("game_key");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {

    }

    public void PollEvents()
    {
      _netManager.PollEvents();
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
      Console.WriteLine("Latencia do Usuário -> {0} {1}ms", peer.Address ,latency);
    }

    public void OnNetworkReceive(
       NetPeer peer,
       NetPacketReader reader,
       byte channelNumber,
       DeliveryMethod deliveryMethod)
    {
      HandlePacket(peer, reader);
      reader.Recycle();
    }

    private void HandlePacket(NetPeer peer, NetPacketReader reader)
    {
      string message = reader.GetString();
      Console.WriteLine("O Cliente me falou -> {0}", message);

      NetDataWriter writer = new NetDataWriter();
      writer.Put("480ba6feccd5d66a39d172e0bf82d2fa76c7b626");

      peer.Send(writer, DeliveryMethod.ReliableOrdered);
      // switch packetType ...
    }

    public void OnPeerConnected(NetPeer peer)
    {
      Console.WriteLine("Usuário Conectado Endereço: {0}", peer.Address);
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
      Console.WriteLine("Usuário -> {0} Desconetado", peer.Address);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }

    //public void PrintStatsAtTop()
    //{
    //  var stats = this._netManager.Statistics;

    //  Console.SetCursorPosition(0, 0);
    //  Console.WriteLine("--------------------------------------------------");
    //  Console.WriteLine($"PACKETS LOSS: {stats.PacketLoss} ms");
    //  Console.WriteLine($"PACKETS SENT: {stats.PacketsSent}");
    //  Console.WriteLine($"PACKETS RECEIVED: {stats.PacketsReceived}");
    //  Console.WriteLine($"BYTES SENT: {stats.BytesSent}");
    //  Console.WriteLine($"BYTES RECEIVED: {stats.BytesReceived}");
    //  Console.WriteLine("--------------------------------------------------");
    //}
  }
}

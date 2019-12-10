namespace NetworkSimpleServer.NetworkLayer.Core.Packets.Serializer
{
    public interface IPacketSerializer
    {
        byte[] Serialize(Packet packet);

        Packet Deserialize(byte[] bytes);
    }
}

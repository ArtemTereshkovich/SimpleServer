namespace NetworkSimpleServer.NetworkLayer.Core.Packets.Formatter
{
    public interface IPacketByteFormatter
    {
        byte[] Serialize(Packet packet);

        Packet Deserialize(byte[] bytes);
    }
}

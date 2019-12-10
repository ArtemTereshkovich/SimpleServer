namespace NetworkSimpleServer.NetworkLayer.Core
{
    public static class PacketConstants
    {
        public static int PacketPayloadThresholdSize = 1024;

        public static int RemoteEndPointSize = 10;

        public static int PacketHeaderSize = 52;

        public static int PacketThresholdSize = PacketPayloadThresholdSize + PacketHeaderSize;

        public static byte[] EmptyPayload = new byte[PacketPayloadThresholdSize];
    }
}

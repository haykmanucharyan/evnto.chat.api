using static evnto.chat.bus.RmqConnector;

namespace evnto.chat.bus
{
    public interface IRmqConnector : IDisposable
    {
        event RmqMessageHandler RmqMessageArrived;

        void ConnectAndInit();

        void PublishGlobal(byte[] message);

        void PublishRouted(byte[] message);

        void BeginConsume();
    }
}
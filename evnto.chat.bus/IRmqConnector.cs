using static evnto.chat.bus.RmqConnector;

namespace evnto.chat.bus
{
    public interface IRmqConnector : IDisposable
    {
        event RmqMessageHandler RmqMessageArrived;

        void PublishGlobal(RmqMessage message);

        void PublishRouted(string routeKey, RmqMessage message);

        void BeginConsume();
    }
}
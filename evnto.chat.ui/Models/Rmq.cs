using Newtonsoft.Json;
using System.Text;

namespace evnto.chat.ui.Models
{
    public enum RmqMessageType : short
    {
        SignIn = 0,
        SignOut = 1,

        ChatCreated = 2,
        ChatStateChanged = 3,

        Message = 4
    }
    public class RmqMessage
    {
        public RmqMessageType MessageType { get; private set; }

        public Dictionary<string, string> PayLoad { get; private set; }

        public RmqMessage(RmqMessageType messageType)
        {
            MessageType = messageType;
            PayLoad = new Dictionary<string, string>();
        }

        public byte[] ToBytes()
        {
            string json = JsonConvert.SerializeObject(this);

            return Encoding.UTF8.GetBytes(json);
        }

        public static RmqMessage FromBytes(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<RmqMessage>(json);
        }
    }
}

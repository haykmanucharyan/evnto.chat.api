namespace evnto.chat.bus
{
    public enum RmqMessageType : short
    {
        SignIn = 0,
        SignOut = 1,

        ChatCreated = 2,
        ChatStateChanged = 3,

        Message = 4
    }
}

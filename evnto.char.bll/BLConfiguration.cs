namespace evnto.chat.bll
{
    public class BLConfiguration
    {
        public string DBConnectionString { get; set; }

        public string RMQConnectionString { get; set; }

        public int SaltMinSeed { get; set; }

        public int SaltRepeatMin { get; set; }

        public int SaltRepeatMax { get; set; }
    }
}

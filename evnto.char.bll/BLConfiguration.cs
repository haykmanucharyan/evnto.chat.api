namespace evnto.chat.bll
{
    public class BLConfiguration
    {
        public string DBConnectionString { get; set; }

        public string RMQHost { get; set; }

        public int RMQPort { get; set; }

        public string RMQUser { get; set; }

        public string RMQPassword { get; set; }

        public int SaltMinSeed { get; set; }

        public int SaltRepeatMin { get; set; }

        public int SaltRepeatMax { get; set; }

        public string ApiKey { get; set; }
    }
}

using System.Security.Cryptography;

namespace evnto.chat.bll.Helpers
{
    internal class SecurityHelper : ISecurityHelper
    {
        int _saltMinSeed;
        int _saltRepeatMin;
        int _saltRepeatMax;

        public SecurityHelper(int saltMinSeed, int saltCountMin, int saltCountMax)
        {
            _saltMinSeed = saltMinSeed;
            _saltRepeatMin = saltCountMin;
            _saltRepeatMax = saltCountMax;
        }

        private byte[] ComputeHash(byte[] data)
        {
            using (SHA256 hasher = SHA256.Create())
                return hasher.ComputeHash(data, 0, data.Length);
        }

        public int GenerateSalt()
        {
            return RandomNumberGenerator.GetInt32(_saltMinSeed, int.MaxValue);
        }

        public int GenerateSaltCount()
        {
            return RandomNumberGenerator.GetInt32(_saltRepeatMin, _saltRepeatMax);
        }

        public string ComputePassowrdHash(string password, int salt, int saltCount)
        {
            byte[] passwordBytes = password.ToBytes();
            byte[] saltBytes = salt.ToBytes();

            byte[] buffer = new byte[passwordBytes.Length + saltCount * saltBytes.Length];

            Buffer.BlockCopy(passwordBytes, 0, buffer, 0, passwordBytes.Length);

            for (int i = 0; i < saltCount; i++)
                Buffer.BlockCopy(saltBytes, 0, buffer, passwordBytes.Length + saltBytes.Length * i, saltBytes.Length);

            return ComputeHash(buffer).ToHexString();
        }
    }
}

using System.Text;

namespace evnto.chat.bll.Helpers
{
    internal static class ByteArrayExtesnions
    {
        public static byte[] ToBytes(this int num)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)(num >> i * 8);

            return bytes;
        }

        public static string ToHexString(this byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);

            foreach (byte b in data)
                sb.AppendFormat("{0:x2}", b);

            return sb.ToString();
        }

        public static byte[] ToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}

namespace Com.HisunCmpay
{
    using System;
    using System.Text;

    public class SignUtil
    {
        public static string HmacSign(string aValue)
        {
            string aKey = "";
            return HmacSign(aValue, aKey);
        }

        public static string HmacSign(string aValue, string aKey)
        {
            int num;
            byte[] data = new byte[0x40];
            byte[] buffer2 = new byte[0x40];
            byte[] bytes = Encoding.UTF8.GetBytes(aKey);
            byte[] buffer4 = Encoding.UTF8.GetBytes(aValue);
            for (num = bytes.Length; num < 0x40; num++)
            {
                data[num] = 0x36;
            }
            for (num = bytes.Length; num < 0x40; num++)
            {
                buffer2[num] = 0x5c;
            }
            for (num = 0; num < bytes.Length; num++)
            {
                data[num] = (byte) (bytes[num] ^ 0x36);
                buffer2[num] = (byte) (bytes[num] ^ 0x5c);
            }
            HmacMD5 cmd = new HmacMD5();
            cmd.update(data, (uint) data.Length);
            cmd.update(buffer4, (uint) buffer4.Length);
            byte[] buffer5 = cmd.finalize();
            cmd.init();
            cmd.update(buffer2, (uint) buffer2.Length);
            cmd.update(buffer5, 0x10);
            return toHex(cmd.finalize());
        }

        public static string toHex(byte[] input)
        {
            if (input == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(input.Length * 2);
            for (int i = 0; i < input.Length; i++)
            {
                int num2 = input[i] & 0xff;
                if (num2 < 0x10)
                {
                    builder.Append("0");
                }
                builder.Append(num2.ToString("x"));
            }
            return builder.ToString();
        }

        public static bool verifySign(string source, string key, string hmac)
        {
            string str2 = HmacSign(HmacSign(source), key);
            return !(string.IsNullOrEmpty(str2) || !str2.Equals(hmac));
        }
    }
}


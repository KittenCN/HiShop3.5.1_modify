namespace Hishop.Plugins.Payment.YeePay
{
    using System;
    using System.Text;

    internal class Digest
    {
        internal Digest()
        {
        }

        internal string HmacSign(string aValue, string aKey)
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
            cmd.Update(data, (uint) data.Length);
            cmd.Update(buffer4, (uint) buffer4.Length);
            byte[] buffer5 = cmd.Finalize1();
            cmd.Init();
            cmd.Update(buffer2, (uint) buffer2.Length);
            cmd.Update(buffer5, 0x10);
            return toHex(cmd.Finalize1());
        }

        internal static string toHex(byte[] input)
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
    }
}


namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BigNum
    {
        private const ulong base32 = 0x100000000L;
        private const ulong base32_10 = 0x3b9aca00L;
        private static readonly ulong[] baseMod = new ulong[] { 1L, 0x1194d800L, 0x2a4ae600L, 0x206c0600L, 0x2dc9fa00L, 0x37957a00L, 0x20ea000L, 0x245faa00L, 0x7ba2600L, 0x16622e00L, 0x52e8c00L, 0x2c7a6400L, 0x3b06e200L, 0x2b443200L, 0x2577e600L, 0x12419e00L };
        private static readonly char[] trimZero = new char[] { '0' };
        private const int wLen = 8;

        public static ulong[] ConvertFromHex(string s)
        {
            int length = s.Length;
            int startIndex = length - 8;
            int num3 = ((length - 1) / 8) + 1;
            ulong[] numArray = new ulong[num3];
            int index = num3 - 1;
            while (startIndex >= 0)
            {
                string str = s.Substring(startIndex, 8);
                numArray[index] = Convert.ToUInt64(str, 0x10);
                index--;
                startIndex -= 8;
            }
            if (startIndex > -8)
            {
                numArray[0] = Convert.ToUInt64(s.Substring(0, startIndex + 8), 0x10);
            }
            return numArray;
        }

        private static ulong[] Div(ulong[] ll)
        {
            List<ulong> list = new List<ulong>();
            int upperBound = ll.GetUpperBound(0);
            int index = 0;
            ulong num3 = 0L;
            ulong num4 = 0L;
            ulong item = 0L;
            bool flag = true;
            while (index <= upperBound)
            {
                num3 = ll[index] + num4;
                num4 = num3 % ((ulong) 0x3b9aca00L);
                item = num3 / ((ulong) 0x3b9aca00L);
                if (!(flag && (item == 0L)))
                {
                    list.Add(item);
                    flag = false;
                }
                num4 *= (ulong) 0x100000000L;
                index++;
            }
            return list.ToArray();
        }

        private static ulong GetBaseMod(int pow)
        {
            return baseMod[pow];
        }

        private static ulong Mod(ulong[] ll)
        {
            int upperBound = ll.GetUpperBound(0);
            int index = 0;
            ulong num3 = 0L;
            ulong num4 = ll[upperBound] % ((ulong) 0x3b9aca00L);
            while (index < upperBound)
            {
                num3 = ll[index] % ((ulong) 0x3b9aca00L);
                num3 *= GetBaseMod(upperBound - index);
                num3 = num3 % ((ulong) 0x3b9aca00L);
                num4 += num3;
                index++;
            }
            return (num4 % ((ulong) 0x3b9aca00L));
        }

        public static string ToDecimalStr(ulong[] ll)
        {
            StringBuilder builder = new StringBuilder();
            List<string> list = new List<string>();
            while ((ll.Length > 0) && (ll[0] != 0L))
            {
                ulong num = Mod(ll);
                ll = Div(ll);
                list.Add(num.ToString("D9"));
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                builder.Append(list[i]);
            }
            if (builder.Length == 0)
            {
                return "0";
            }
            return builder.ToString().TrimStart(trimZero);
        }
    }
}


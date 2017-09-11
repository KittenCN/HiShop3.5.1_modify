namespace Hishop.Plugins.Payment.YeePay
{
    using System;

    internal class HmacMD5
    {
        private byte[] buffer = new byte[0x40];
        private uint[] count = new uint[2];
        private byte[] digest = new byte[0x10];
        private static byte[] pad;
        private const uint S11 = 7;
        private const uint S12 = 12;
        private const uint S13 = 0x11;
        private const uint S14 = 0x16;
        private const uint S21 = 5;
        private const uint S22 = 9;
        private const uint S23 = 14;
        private const uint S24 = 20;
        private const uint S31 = 4;
        private const uint S32 = 11;
        private const uint S33 = 0x10;
        private const uint S34 = 0x17;
        private const uint S41 = 6;
        private const uint S42 = 10;
        private const uint S43 = 15;
        private const uint S44 = 0x15;
        private uint[] state = new uint[4];

        static HmacMD5()
        {
            byte[] buffer = new byte[0x40];
            buffer[0] = 0x80;
            pad = buffer;
        }

        internal HmacMD5()
        {
            this.Init();
        }

        private static void Decode(ref uint[] output, byte[] input, uint len)
        {
            uint num;
            uint num2;
            if (BitConverter.IsLittleEndian)
            {
                num = 0;
                for (num2 = 0; num2 < len; num2 += 4)
                {
                    output[num] = (uint) (((input[num2] | (input[(int) ((IntPtr) (num2 + 1))] << 8)) | (input[(int) ((IntPtr) (num2 + 2))] << 0x10)) | (input[(int) ((IntPtr) (num2 + 3))] << 0x18));
                    num++;
                }
            }
            else
            {
                num = 0;
                for (num2 = 0; num2 < len; num2 += 4)
                {
                    output[num] = (uint) (((input[(int) ((IntPtr) (num2 + 3))] | (input[(int) ((IntPtr) (num2 + 2))] << 8)) | (input[(int) ((IntPtr) (num2 + 1))] << 0x10)) | (input[num2] << 0x18));
                    num++;
                }
            }
        }

        private static void Encode(ref byte[] output, uint[] input, uint len)
        {
            uint num;
            uint num2;
            if (BitConverter.IsLittleEndian)
            {
                num = 0;
                for (num2 = 0; num2 < len; num2 += 4)
                {
                    output[num2] = (byte) (input[num] & 0xff);
                    output[(int) ((IntPtr) (num2 + 1))] = (byte) ((input[num] >> 8) & 0xff);
                    output[(int) ((IntPtr) (num2 + 2))] = (byte) ((input[num] >> 0x10) & 0xff);
                    output[(int) ((IntPtr) (num2 + 3))] = (byte) ((input[num] >> 0x18) & 0xff);
                    num++;
                }
            }
            else
            {
                num = 0;
                for (num2 = 0; num2 < len; num2 += 4)
                {
                    output[(int) ((IntPtr) (num2 + 3))] = (byte) (input[num] & 0xff);
                    output[(int) ((IntPtr) (num2 + 2))] = (byte) ((input[num] >> 8) & 0xff);
                    output[(int) ((IntPtr) (num2 + 1))] = (byte) ((input[num] >> 0x10) & 0xff);
                    output[num2] = (byte) ((input[num] >> 0x18) & 0xff);
                    num++;
                }
            }
        }

        private uint F(uint x, uint y, uint z)
        {
            return ((x & y) | (~x & z));
        }

        private void FF(ref uint a, uint b, uint c, uint d, uint x, uint s, uint ac)
        {
            a += (this.F(b, c, d) + x) + ac;
            a = RotateLeft(a, s) + b;
        }

        public byte[] Finalize1()
        {
            byte[] output = new byte[8];
            Encode(ref output, this.count, 8);
            uint num = (this.count[0] >> 3) & 0x3f;
            uint length = (num < 0x38) ? (0x38 - num) : (120 - num);
            this.Update(pad, length);
            this.Update(output, 8);
            Encode(ref this.digest, this.state, 0x10);
            for (int i = 0; i < 0x40; i++)
            {
                this.buffer[i] = 0;
            }
            return this.digest;
        }

        private uint G(uint x, uint y, uint z)
        {
            return ((x & z) | (y & ~z));
        }

        private void GG(ref uint a, uint b, uint c, uint d, uint x, uint s, uint ac)
        {
            a += (this.G(b, c, d) + x) + ac;
            a = RotateLeft(a, s) + b;
        }

        private uint H(uint x, uint y, uint z)
        {
            return ((x ^ y) ^ z);
        }

        private void HH(ref uint a, uint b, uint c, uint d, uint x, uint s, uint ac)
        {
            a += (this.H(b, c, d) + x) + ac;
            a = RotateLeft(a, s) + b;
        }

        private uint I(uint x, uint y, uint z)
        {
            return (y ^ (x | ~z));
        }

        private void II(ref uint a, uint b, uint c, uint d, uint x, uint s, uint ac)
        {
            a += (this.I(b, c, d) + x) + ac;
            a = RotateLeft(a, s) + b;
        }

        internal void Init()
        {
            this.count[0] = 0;
            this.count[1] = 0;
            this.state[0] = 0x67452301;
            this.state[1] = 0xefcdab89;
            this.state[2] = 0x98badcfe;
            this.state[3] = 0x10325476;
        }

        internal string Md5String()
        {
            string str = "";
            for (int i = 0; i < this.digest.Length; i++)
            {
                str = str + this.digest[i].ToString("x2");
            }
            return str;
        }

        private static uint RotateLeft(uint x, uint n)
        {
            return x << (int)n | x >> 32 - (int)n;
        }

        private void Transform(byte[] data)
        {
            uint a = this.state[0];
            uint b = this.state[1];
            uint c = this.state[2];
            uint d = this.state[3];
            uint[] output = new uint[0x10];
            Decode(ref output, data, 0x40);
            this.FF(ref a, b, c, d, output[0], 7, 0xd76aa478);
            this.FF(ref d, a, b, c, output[1], 12, 0xe8c7b756);
            this.FF(ref c, d, a, b, output[2], 0x11, 0x242070db);
            this.FF(ref b, c, d, a, output[3], 0x16, 0xc1bdceee);
            this.FF(ref a, b, c, d, output[4], 7, 0xf57c0faf);
            this.FF(ref d, a, b, c, output[5], 12, 0x4787c62a);
            this.FF(ref c, d, a, b, output[6], 0x11, 0xa8304613);
            this.FF(ref b, c, d, a, output[7], 0x16, 0xfd469501);
            this.FF(ref a, b, c, d, output[8], 7, 0x698098d8);
            this.FF(ref d, a, b, c, output[9], 12, 0x8b44f7af);
            this.FF(ref c, d, a, b, output[10], 0x11, 0xffff5bb1);
            this.FF(ref b, c, d, a, output[11], 0x16, 0x895cd7be);
            this.FF(ref a, b, c, d, output[12], 7, 0x6b901122);
            this.FF(ref d, a, b, c, output[13], 12, 0xfd987193);
            this.FF(ref c, d, a, b, output[14], 0x11, 0xa679438e);
            this.FF(ref b, c, d, a, output[15], 0x16, 0x49b40821);
            this.GG(ref a, b, c, d, output[1], 5, 0xf61e2562);
            this.GG(ref d, a, b, c, output[6], 9, 0xc040b340);
            this.GG(ref c, d, a, b, output[11], 14, 0x265e5a51);
            this.GG(ref b, c, d, a, output[0], 20, 0xe9b6c7aa);
            this.GG(ref a, b, c, d, output[5], 5, 0xd62f105d);
            this.GG(ref d, a, b, c, output[10], 9, 0x2441453);
            this.GG(ref c, d, a, b, output[15], 14, 0xd8a1e681);
            this.GG(ref b, c, d, a, output[4], 20, 0xe7d3fbc8);
            this.GG(ref a, b, c, d, output[9], 5, 0x21e1cde6);
            this.GG(ref d, a, b, c, output[14], 9, 0xc33707d6);
            this.GG(ref c, d, a, b, output[3], 14, 0xf4d50d87);
            this.GG(ref b, c, d, a, output[8], 20, 0x455a14ed);
            this.GG(ref a, b, c, d, output[13], 5, 0xa9e3e905);
            this.GG(ref d, a, b, c, output[2], 9, 0xfcefa3f8);
            this.GG(ref c, d, a, b, output[7], 14, 0x676f02d9);
            this.GG(ref b, c, d, a, output[12], 20, 0x8d2a4c8a);
            this.HH(ref a, b, c, d, output[5], 4, 0xfffa3942);
            this.HH(ref d, a, b, c, output[8], 11, 0x8771f681);
            this.HH(ref c, d, a, b, output[11], 0x10, 0x6d9d6122);
            this.HH(ref b, c, d, a, output[14], 0x17, 0xfde5380c);
            this.HH(ref a, b, c, d, output[1], 4, 0xa4beea44);
            this.HH(ref d, a, b, c, output[4], 11, 0x4bdecfa9);
            this.HH(ref c, d, a, b, output[7], 0x10, 0xf6bb4b60);
            this.HH(ref b, c, d, a, output[10], 0x17, 0xbebfbc70);
            this.HH(ref a, b, c, d, output[13], 4, 0x289b7ec6);
            this.HH(ref d, a, b, c, output[0], 11, 0xeaa127fa);
            this.HH(ref c, d, a, b, output[3], 0x10, 0xd4ef3085);
            this.HH(ref b, c, d, a, output[6], 0x17, 0x4881d05);
            this.HH(ref a, b, c, d, output[9], 4, 0xd9d4d039);
            this.HH(ref d, a, b, c, output[12], 11, 0xe6db99e5);
            this.HH(ref c, d, a, b, output[15], 0x10, 0x1fa27cf8);
            this.HH(ref b, c, d, a, output[2], 0x17, 0xc4ac5665);
            this.II(ref a, b, c, d, output[0], 6, 0xf4292244);
            this.II(ref d, a, b, c, output[7], 10, 0x432aff97);
            this.II(ref c, d, a, b, output[14], 15, 0xab9423a7);
            this.II(ref b, c, d, a, output[5], 0x15, 0xfc93a039);
            this.II(ref a, b, c, d, output[12], 6, 0x655b59c3);
            this.II(ref d, a, b, c, output[3], 10, 0x8f0ccc92);
            this.II(ref c, d, a, b, output[10], 15, 0xffeff47d);
            this.II(ref b, c, d, a, output[1], 0x15, 0x85845dd1);
            this.II(ref a, b, c, d, output[8], 6, 0x6fa87e4f);
            this.II(ref d, a, b, c, output[15], 10, 0xfe2ce6e0);
            this.II(ref c, d, a, b, output[6], 15, 0xa3014314);
            this.II(ref b, c, d, a, output[13], 0x15, 0x4e0811a1);
            this.II(ref a, b, c, d, output[4], 6, 0xf7537e82);
            this.II(ref d, a, b, c, output[11], 10, 0xbd3af235);
            this.II(ref c, d, a, b, output[2], 15, 0x2ad7d2bb);
            this.II(ref b, c, d, a, output[9], 0x15, 0xeb86d391);
            this.state[0] += a;
            this.state[1] += b;
            this.state[2] += c;
            this.state[3] += d;
            for (int i = 0; i < 0x10; i++)
            {
                output[i] = 0;
            }
        }

        internal void Update(byte[] data, uint length)
        {
            uint num = length;
            uint num2 = (this.count[0] >> 3) & 0x3f;
            uint num3 = length << 3;
            uint num4 = 0;
            if (length > 0)
            {
                this.count[0] += num3;
                this.count[1] += length >> 0x1d;
                if (this.count[0] < num3)
                {
                    this.count[1]++;
                }
                if (num2 > 0)
                {
                    uint num5 = 0x40 - num2;
                    uint num6 = ((num2 + length) > 0x40) ? (0x40 - num2) : length;
                    Buffer.BlockCopy(data, 0, this.buffer, (int) num2, (int) num6);
                    if ((num2 + num6) < 0x40)
                    {
                        return;
                    }
                    this.Transform(this.buffer);
                    num4 += num6;
                    num -= num6;
                }
                while (num >= 0x40)
                {
                    Buffer.BlockCopy(data, (int) num4, this.buffer, 0, 0x40);
                    this.Transform(this.buffer);
                    num4 += 0x40;
                    num -= 0x40;
                }
                if (num > 0)
                {
                    Buffer.BlockCopy(data, (int) num4, this.buffer, 0, (int) num);
                }
            }
        }
    }
}


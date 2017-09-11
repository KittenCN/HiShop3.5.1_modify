namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MsgList
    {
        public string UserOpenId;
        public string RealName;
        public string RoleName;
        public int Msg1;
        public int Msg2;
        public int Msg3;
        public int Msg4;
        public int Msg5;
        public int Msg6;
        public int Type;
    }
}


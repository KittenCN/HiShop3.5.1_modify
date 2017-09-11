namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class AbstractResponseMessage
    {
        protected AbstractResponseMessage()
        {
        }

        public int MaterialId { get; set; }

        public string MsgType { get; set; }
    }
}


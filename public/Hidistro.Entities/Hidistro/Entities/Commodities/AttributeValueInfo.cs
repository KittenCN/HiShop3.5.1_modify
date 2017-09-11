namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AttributeValueInfo
    {
        public int AttributeId { get; set; }

        public int DisplaySequence { get; set; }

        public string ImageUrl { get; set; }

        public int ValueId { get; set; }

        public string ValueStr { get; set; }
    }
}


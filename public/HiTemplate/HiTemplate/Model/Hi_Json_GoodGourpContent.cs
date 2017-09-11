namespace HiTemplate.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Hi_Json_GoodGourpContent
    {
        public int goodsize { get; set; }

        public IList<HiShop_Model_Good> goodslist { get; set; }

        public GoodGourp group { get; set; }

        public int layout { get; set; }

        public int secondPriority { get; set; }

        public bool showIco { get; set; }

        public bool showMaketPrice { get; set; }

        public bool showName { get; set; }

        public bool showPrice { get; set; }
    }
}


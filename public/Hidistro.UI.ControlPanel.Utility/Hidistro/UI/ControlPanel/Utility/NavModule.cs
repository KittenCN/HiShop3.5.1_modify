namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Collections.Generic;

    public class NavModule
    {
        public string Class;
        public string ID;
        public bool IsDivide;
        public Dictionary<string, NavItem> ItemList = new Dictionary<string, NavItem>();
        public string Link;
        public string Title;
    }
}


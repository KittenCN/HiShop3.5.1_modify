namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Collections.Generic;

    public class NavItem
    {
        public string Class;
        public string ID;
        public Dictionary<string, NavPageLink> PageLinks = new Dictionary<string, NavPageLink>();
        public string SpanName;
    }
}


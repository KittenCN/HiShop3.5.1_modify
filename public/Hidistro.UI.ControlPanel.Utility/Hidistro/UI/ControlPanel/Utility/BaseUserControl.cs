namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI;

    public class BaseUserControl : UserControl
    {
        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("HiTipsShow(\"{0}\", {1})", msg, success ? "'success'" : "'error'");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }
    }
}


using System;
using System.Web;

public class ConfigHandler : Handler
{
    public ConfigHandler(HttpContext context) : base(context)
    {
    }

    public override void Process()
    {
        base.WriteJson(Config.Items);
    }
}


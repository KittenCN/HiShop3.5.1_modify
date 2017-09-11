using System;
using System.Web;

public class NotSupportedHandler : Handler
{
    public NotSupportedHandler(HttpContext context) : base(context)
    {
    }

    public override void Process()
    {
        base.WriteJson(new { state = "action 参数为空或者 action 不被支持。" });
    }
}


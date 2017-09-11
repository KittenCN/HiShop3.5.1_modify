using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Web;

public abstract class Handler
{
    public Handler(HttpContext context)
    {
        this.Request = context.Request;
        this.Response = context.Response;
        this.Context = context;
        this.Server = context.Server;
    }

    public abstract void Process();
    protected void WriteJson(object response)
    {
        string str = this.Request["callback"];
        string s = JsonConvert.SerializeObject(response);
        if (string.IsNullOrWhiteSpace(str))
        {
            this.Response.AddHeader("Content-Type", "text/plain");
            this.Response.Write(s);
        }
        else
        {
            this.Response.AddHeader("Content-Type", "application/javascript");
            this.Response.Write(string.Format("{0}({1});", str, s));
        }
        this.Response.End();
    }

    public HttpContext Context { get; private set; }

    public HttpRequest Request { get; private set; }

    public HttpResponse Response { get; private set; }

    public HttpServerUtility Server { get; private set; }
}


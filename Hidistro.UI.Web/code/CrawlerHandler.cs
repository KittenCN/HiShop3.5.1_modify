﻿using System;
using System.Linq;
using System.Web;
namespace Hidistro.UI.Web
{
    public class CrawlerHandler : Handler
    {
        private Crawler[] Crawlers;
        private string[] Sources;

        public CrawlerHandler(HttpContext context) : base(context)
        {
        }

        public override void Process()
        {
            this.Sources = base.Request.Form.GetValues("source[]");
            if ((this.Sources == null) || (this.Sources.Length == 0))
            {
                base.WriteJson(new { state = "参数错误：没有指定抓取源" });
            }
            else
            {
                this.Crawlers = (from x in this.Sources select new Crawler(x, base.Server).Fetch()).ToArray<Crawler>();
                base.WriteJson(new { state = "SUCCESS", list = from x in this.Crawlers select new { state = x.State, source = x.SourceUrl, url = x.ServerUrl } });
            }
        }
    }
}
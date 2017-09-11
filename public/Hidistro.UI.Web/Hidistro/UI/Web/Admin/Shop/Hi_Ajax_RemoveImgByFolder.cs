namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;

    public class Hi_Ajax_RemoveImgByFolder : IHttpHandler
    {
        public string MoveImgByFolder(HttpContext context)
        {
            DbQueryResult result = GalleryHelper.GetPhotoList("", new int?(Convert.ToInt32(context.Request.Form["cid"])), 1, 0x5f5e100, PhotoListOrder.UploadTimeDesc, 0);
            List<int> pList = new List<int>();
            DataTable data = (DataTable) result.Data;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                pList.Add(Convert.ToInt32(data.Rows[i]["PhotoId"]));
            }
            if (GalleryHelper.MovePhotoType(pList, Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
            {
                return "{\"status\":1,\"msg\":\"\"}";
            }
            return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(this.MoveImgByFolder(context));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


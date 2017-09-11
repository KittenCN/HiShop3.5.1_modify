namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ImageTypeLabel : Literal
    {
        private int typeId = -1;

        protected override void Render(HtmlTextWriter writer)
        {
            string str = "<ul>";
            string str2 = string.Empty;
            DataTable photoCategories = GalleryHelper.GetPhotoCategories(this.typeId);
            int defaultPhotoCount = GalleryHelper.GetDefaultPhotoCount();
            string str3 = this.Page.Request.QueryString["ImageTypeId"];
            if (!string.IsNullOrEmpty(str3))
            {
                str3 = this.Page.Request.QueryString["ImageTypeId"];
            }
            if (str3 == "0")
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<li><a href=\"ImageData.aspx?ImageTypeId=0\"><s></s><strong>默认分类<span>(", defaultPhotoCount, ")</span></strong></a></li>" });
            }
            else
            {
                object obj3 = str;
                str = string.Concat(new object[] { obj3, "<li><a href=\"ImageData.aspx?ImageTypeId=0\"><s></s>默认分组<span>(", defaultPhotoCount, ")</span></a></li>" });
            }
            foreach (DataRow row in photoCategories.Rows)
            {
                if (row["CategoryId"].ToString() == str3)
                {
                    str2 = string.Format("<li><a href=\"ImageData.aspx?ImageTypeId={0}\"><s></s><strong>{1}</strong><span>({2})</span></a></li>", row["CategoryId"], row["CategoryName"], row["PhotoCounts"].ToString());
                }
                else
                {
                    str2 = string.Format("<li><a href=\"ImageData.aspx?ImageTypeId={0}\"><s></s>{1}<span>({2})</span></a></li>", row["CategoryId"], row["CategoryName"], row["PhotoCounts"].ToString());
                }
                str = str + str2;
            }
            str = str + "</ul>";
            base.Text = str;
            base.Render(writer);
        }

        public int TypeId
        {
            get
            {
                return this.typeId;
            }
            set
            {
                this.typeId = value;
            }
        }
    }
}


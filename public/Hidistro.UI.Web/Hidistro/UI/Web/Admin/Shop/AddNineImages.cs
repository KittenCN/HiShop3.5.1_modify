namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Web.UI.WebControls;

    public class AddNineImages : AdminPage
    {
        protected Literal EditType;
        protected int nid;

        protected AddNineImages() : base("m01", "dpp10")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["task"];
            if (!string.IsNullOrEmpty(str))
            {
                string str2 = "未定义操作";
                string str3 = base.Request.Form["ShareDesc"];
                string str4 = base.Request.Form["image1"];
                string str5 = base.Request.Form["image2"];
                string str6 = base.Request.Form["image3"];
                string str7 = base.Request.Form["image4"];
                string str8 = base.Request.Form["image5"];
                string str9 = base.Request.Form["image6"];
                string str10 = base.Request.Form["image7"];
                string str11 = base.Request.Form["image8"];
                string str12 = base.Request.Form["image9"];
                string str13 = base.Request.Form["ID"];
                NineImgsesItem info = new NineImgsesItem {
                    CreatTime = DateTime.Now,
                    ShareDesc = str3,
                    image1 = str4,
                    image2 = str5,
                    image3 = str6,
                    image4 = str7,
                    image5 = str8,
                    image6 = str9,
                    image7 = str10,
                    image8 = str11,
                    image9 = str12
                };
                int result = 0;
                int.TryParse(str13, out result);
                string str15 = str;
                if (str15 != null)
                {
                    if (!(str15 == "del"))
                    {
                        if (str15 == "read")
                        {
                            if (result == 0)
                            {
                                str2 = "falid：参数不正确";
                            }
                            else
                            {
                                NineImgsesItem nineImgse = ShareMaterialBrowser.GetNineImgse(result);
                                if (nineImgse != null)
                                {
                                    IsoDateTimeConverter converter = new IsoDateTimeConverter {
                                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                                    };
                                    str2 = JsonConvert.SerializeObject(nineImgse, new JsonConverter[] { converter });
                                }
                                else
                                {
                                    str2 = "falid：素材已删除";
                                }
                            }
                        }
                        else if (str15 == "edit")
                        {
                            if (result == 0)
                            {
                                str2 = "ID参数不正确";
                            }
                            else
                            {
                                info.id = result;
                                if (ShareMaterialBrowser.UpdateNineImgses(info))
                                {
                                    str2 = "success";
                                }
                                else
                                {
                                    str2 = "修改失败！";
                                }
                            }
                        }
                        else if (str15 == "add")
                        {
                            if (ShareMaterialBrowser.AddNineImgses(info) > 0)
                            {
                                str2 = "success";
                            }
                            else
                            {
                                str2 = "保存失败！";
                            }
                        }
                    }
                    else if (result == 0)
                    {
                        str2 = "falid：参数不正确";
                    }
                    else if (ShareMaterialBrowser.DeleteNineImgses(result))
                    {
                        str2 = "success";
                    }
                    else
                    {
                        str2 = "falid：删除失败";
                    }
                }
                base.Response.Write(str2);
                base.Response.End();
            }
            string s = base.Request.QueryString["ID"];
            this.nid = 0;
            if (int.TryParse(s, out this.nid))
            {
                this.EditType.Text = "编辑";
            }
            else
            {
                this.EditType.Text = "新增";
            }
        }
    }
}


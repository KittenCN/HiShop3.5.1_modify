namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Settings;
    using Hidistro.Entities.Settings;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Web.UI.HtmlControls;

    public class EditShippingTemplate : AdminPage
    {
        public string SName;
        private SysTaskMsg TaskMsg;
        public string TemplateId;
        protected HtmlForm thisForm;

        protected EditShippingTemplate() : base("m09", "szp06")
        {
            this.TaskMsg = new SysTaskMsg();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.TemplateId = base.Request["TemplateId"];
            this.SName = base.Request["TemplateName"];
            if (string.IsNullOrEmpty(this.TemplateId))
            {
                this.ShowMsg("读取数据异常，无法编辑", false);
            }
            else
            {
                string str = base.Request["task"];
                if (!string.IsNullOrEmpty(str))
                {
                    if (str == "ReadData")
                    {
                        FreightTemplate freightTemplate = SettingsHelper.GetFreightTemplate(int.Parse(this.TemplateId), true);
                        base.Response.ContentType = "text/json";
                        base.Response.Write(JsonConvert.SerializeObject(freightTemplate));
                        base.Response.End();
                    }
                    else if (str == "EditSave")
                    {
                        this.TaskMsg.state = "faild";
                        this.TaskMsg.msg = "未知错误";
                        NameValueCollection form = new NameValueCollection();
                        form.Add(base.Request.Form);
                        FreightTemplate template2 = this.ReadPostData(form);
                        if (this.TaskMsg.state == "success")
                        {
                            string templateName = base.Request.Form["sName"];
                            if (SettingsHelper.UpdateShippingTemplate(template2, templateName))
                            {
                                this.TaskMsg.msg = "模板修改成功！";
                            }
                            else
                            {
                                this.TaskMsg.state = "faild";
                                if (SettingsHelper.Error != "")
                                {
                                    this.TaskMsg.msg = "模板修改失败:" + SettingsHelper.Error;
                                }
                                else
                                {
                                    this.TaskMsg.msg = "该模板名称已存在,请重新输入名称!";
                                }
                            }
                        }
                        base.Response.Write(JsonConvert.SerializeObject(this.TaskMsg));
                        base.Response.End();
                    }
                }
            }
        }

        private FreightTemplate ReadPostData(NameValueCollection Form)
        {
            string str = "";
            FreightTemplate template = new FreightTemplate();
            try
            {
                this.TaskMsg.msg = "";
                template.Name = Form["Name"];
                template.TemplateId = int.Parse(Form["TemplateId"]);
                if (!string.IsNullOrEmpty(template.Name))
                {
                    template.MUnit = int.Parse(Form["MUnit"]);
                    if (int.Parse(Form["FreeShip"]) == 0)
                    {
                        template.FreeShip = false;
                    }
                    else
                    {
                        template.FreeShip = true;
                    }
                    if (!template.FreeShip)
                    {
                        ValidationResults results;
                        if (int.Parse(Form["HasFree"]) == 0)
                        {
                            template.HasFree = false;
                        }
                        else
                        {
                            template.HasFree = true;
                            int num = 0;
                            template.FreeShippings = new List<FreeShipping>();
                            while (!string.IsNullOrEmpty(Form["freeShippings[" + num + "][ModelId]"]) && (this.TaskMsg.msg == ""))
                            {
                                FreeShipping target = new FreeShipping {
                                    ModeId = int.Parse(Form["freeShippings[" + num + "][ModelId]"]),
                                    ConditionType = int.Parse(Form["freeShippings[" + num + "][ConditionType]"]),
                                    ConditionNumber = Form["freeShippings[" + num + "][ConditionNumber]"]
                                };
                                string str2 = Form["freeShippings[" + num + "][FreeRegions]"];
                                if (!string.IsNullOrEmpty(str2))
                                {
                                    string[] strArray = str2.Split(new char[] { ',' });
                                    if (strArray.Length > 0)
                                    {
                                        target.FreeShippingRegions = new List<FreeShippingRegion>();
                                        foreach (string str3 in strArray)
                                        {
                                            int num2 = 0;
                                            if (int.TryParse(str3, out num2) && (num2 != 0))
                                            {
                                                FreeShippingRegion item = new FreeShippingRegion {
                                                    RegionId = num2
                                                };
                                                target.FreeShippingRegions.Add(item);
                                            }
                                        }
                                    }
                                }
                                results = Hishop.Components.Validation.Validation.Validate<FreeShipping>(target, new string[] { "ValFree" });
                                str = "";
                                if (!results.IsValid)
                                {
                                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                                    {
                                        str = str + result.Message;
                                    }
                                    this.TaskMsg.msg = str;
                                }
                                template.FreeShippings.Add(target);
                                num++;
                            }
                        }
                        int num3 = 0;
                        if (!string.IsNullOrEmpty(Form["shipperSelect[" + num3 + "][ModelId]"]) && (this.TaskMsg.msg == ""))
                        {
                            template.SpecifyRegionGroups = new List<SpecifyRegionGroup>();
                            while (!string.IsNullOrEmpty(Form["shipperSelect[" + num3 + "][ModelId]"]) && (this.TaskMsg.msg == ""))
                            {
                                SpecifyRegionGroup group = new SpecifyRegionGroup {
                                    ModeId = int.Parse(Form["shipperSelect[" + num3 + "][ModelId]"]),
                                    FristPrice = decimal.Parse(Form["shipperSelect[" + num3 + "][FristPrice]"]),
                                    AddNumber = decimal.Parse(Form["shipperSelect[" + num3 + "][AddNumber]"]),
                                    FristNumber = decimal.Parse(Form["shipperSelect[" + num3 + "][FristNumber]"]),
                                    AddPrice = decimal.Parse(Form["shipperSelect[" + num3 + "][AddPrice]"]),
                                    IsDefault = false
                                };
                                if (int.Parse(Form["shipperSelect[" + num3 + "][IsDefault]"]) == 1)
                                {
                                    group.IsDefault = true;
                                }
                                string str4 = Form["shipperSelect[" + num3 + "][SpecifyRegions]"];
                                if (!string.IsNullOrEmpty(str4))
                                {
                                    string[] strArray2 = str4.Split(new char[] { ',' });
                                    if (strArray2.Length > 0)
                                    {
                                        group.SpecifyRegions = new List<SpecifyRegion>();
                                        foreach (string str5 in strArray2)
                                        {
                                            int num4 = 0;
                                            if (int.TryParse(str5, out num4) && (num4 != 0))
                                            {
                                                SpecifyRegion region2 = new SpecifyRegion {
                                                    RegionId = num4
                                                };
                                                group.SpecifyRegions.Add(region2);
                                            }
                                        }
                                    }
                                }
                                results = Hishop.Components.Validation.Validation.Validate<SpecifyRegionGroup>(group, new string[] { "ValRegionGroup" });
                                str = "";
                                if (!results.IsValid)
                                {
                                    foreach (ValidationResult result2 in (IEnumerable<ValidationResult>) results)
                                    {
                                        str = str + result2.Message;
                                    }
                                    this.TaskMsg.msg = str;
                                }
                                template.SpecifyRegionGroups.Add(group);
                                num3++;
                            }
                        }
                        else
                        {
                            this.TaskMsg.msg = "没有运送方式选择";
                        }
                    }
                    else
                    {
                        template.HasFree = false;
                    }
                }
                else
                {
                    this.TaskMsg.msg = "模板名称不能为空";
                }
                if (this.TaskMsg.msg == "")
                {
                    this.TaskMsg.state = "success";
                    return template;
                }
                this.TaskMsg.state = "faild";
            }
            catch (Exception exception)
            {
                this.TaskMsg.msg = "参数异常：" + exception.Message.ToString();
            }
            return template;
        }

        private class SysTaskMsg
        {
            public string msg { get; set; }

            public string state { get; set; }
        }
    }
}


namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class SaveVoteHandler : IHttpHandler
    {
        private string IntToChar(int i)
        {
            char ch = (char) (i + 0x43);
            return ch.ToString();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                int num = int.Parse(context.Request["id"].ToString());
                string str = context.Request["name"].ToString();
                string val = context.Request["begin"].ToString();
                string str3 = context.Request["end"].ToString();
                string str4 = context.Request["memberlvl"].ToString();
                string str5 = context.Request["defualtgroup"].ToString();
                string str6 = context.Request["customgroup"].ToString();
                string str7 = context.Request["img"].ToString();
                string str8 = context.Request["des"].ToString();
                bool flag = bool.Parse(context.Request["ismulti"].ToString());
                int num2 = int.Parse(context.Request["maxcheck"].ToString());
                string str9 = context.Request["items"].ToString();
                DateTime now = DateTime.Now;
                DateTime i = DateTime.Now;
                if (string.IsNullOrEmpty(str))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"投票标题不能为空\"}");
                }
                else if (str.Length > 60)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"投票标题不能超过60个字符\"}");
                }
                else if (string.IsNullOrEmpty(str9))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"投票选项不能为空\"}");
                }
                else if (string.IsNullOrEmpty(str7))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"没有上传活动封面\"}");
                }
                else if (!val.bDate(ref now))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的开始时间\"}");
                }
                else if (!str3.bDate(ref i))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的结束时间\"}");
                }
                else if (i <= now)
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"结束时间要大于开始时间\"}");
                }
                else if (string.IsNullOrEmpty(str8))
                {
                    context.Response.Write("{\"type\":\"error\",\"data\":\"活动说明不能为空\"}");
                }
                else
                {
                    VoteInfo vote = new VoteInfo();
                    if (num != 0)
                    {
                        vote = VoteHelper.GetVote((long) num);
                        if (vote == null)
                        {
                            context.Response.Write("{\"type\":\"error\",\"data\":\"没有找到这个调查\"}");
                            return;
                        }
                    }
                    List<VoteItemInfo> list = new List<VoteItemInfo>();
                    if (!string.IsNullOrEmpty(str9))
                    {
                        string[] strArray = str9.Split(new char[] { ',' });
                        if (strArray.Length > 0)
                        {
                            for (int j = 0; j < strArray.Length; j++)
                            {
                                VoteItemInfo item = new VoteItemInfo();
                                if (num > 0)
                                {
                                    item.VoteId = num;
                                }
                                item.ItemCount = 0;
                                item.VoteItemName = strArray[j];
                                list.Add(item);
                            }
                        }
                    }
                    vote.VoteName = str;
                    vote.EndDate = i;
                    vote.StartDate = now;
                    vote.MemberGrades = str4;
                    vote.DefualtGroup = str5;
                    vote.CustomGroup = str6;
                    vote.Description = str8;
                    vote.ImageUrl = str7;
                    vote.IsMultiCheck = flag;
                    vote.MaxCheck = num2;
                    vote.VoteItems = list;
                    long voteId = 0L;
                    if (num == 0)
                    {
                        voteId = VoteHelper.Create(vote);
                    }
                    else
                    {
                        voteId = vote.VoteId;
                        if (!VoteHelper.Update(vote, true))
                        {
                            voteId = 0L;
                        }
                    }
                    if (voteId > 0L)
                    {
                        context.Response.Write("{\"type\":\"success\",\"data\":\"" + voteId.ToString() + "\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"type\":\"error\",\"data\":\"写数据库出错\"}");
                    }
                }
            }
            catch (Exception exception)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"" + exception.Message + "\"}");
            }
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


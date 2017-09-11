namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using global::ControlPanel.WeiBo;
    using global::ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VFriendsCircle : VshopTemplatedWebControl
    {
        private Literal ItemHtml;
        private Pager pager;
        private VshopTemplatedRepeater refriendscircle;

        protected override void AttachChildControls()
        {
            this.refriendscircle = (VshopTemplatedRepeater) this.FindControl("refriendscircle");
            this.ItemHtml = (Literal) this.FindControl("ItemHtml");
            this.pager = (Pager) this.FindControl("pager");
            this.refriendscircle.ItemDataBound += new RepeaterItemEventHandler(this.refriendscircle_ItemDataBound);
            this.BindData();
            PageTitle.AddSiteNameTitle("朋友圈素材");
        }

        private void BindData()
        {
            ArticleQuery entity = new ArticleQuery {
                SortBy = "ArticleId",
                SortOrder = SortAction.Desc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.IsShare = 1;
            DbQueryResult articleRequest = ArticleHelper.GetArticleRequest(entity);
            this.refriendscircle.DataSource = articleRequest.Data;
            this.refriendscircle.DataBind();
            this.pager.TotalRecords = articleRequest.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-VFriendsCircle.html";
            }
            base.OnInit(e);
        }

        private void refriendscircle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater repeater = e.Item.Controls[0].FindControl("ItemInfo") as Repeater;
                Literal literal = e.Item.Controls[0].FindControl("ItemHtml") as Literal;
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                if (dataItem["ArticleType"].ToString() == "4")
                {
                    IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(dataItem["ArticleId"].ToString()));
                    if (articleItems != null)
                    {
                        repeater.DataSource = articleItems;
                        repeater.DataBind();
                    }
                }
                else
                {
                    literal.Text = "<div class='mate-ctx clear' >" + dataItem["Memo"].ToString() + "</div>";
                }
            }
        }
    }
}


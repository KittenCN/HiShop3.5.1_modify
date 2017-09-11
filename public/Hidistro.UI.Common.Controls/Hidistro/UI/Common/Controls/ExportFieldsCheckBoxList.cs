namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList : CheckBoxList
    {
        private int repeatColumns = 9;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("昵称", "UserName"));
            this.Items.Add(new ListItem("姓名", "RealName"));
            this.Items.Add(new ListItem("手机号", "CellPhone"));
            this.Items.Add(new ListItem("QQ", "QQ"));
            this.Items.Add(new ListItem("邮箱", "Email"));
            this.Items.Add(new ListItem("OpenId", "Openid"));
            this.Items.Add(new ListItem("积分", "Points"));
            this.Items.Add(new ListItem("消费金额", "Expenditure"));
            this.Items.Add(new ListItem("详细地址", "Address"));
        }

        public override int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}


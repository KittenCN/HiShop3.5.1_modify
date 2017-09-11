namespace Hidistro.UI.Web.Admin
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SetMemberRange : UserControl
    {
        private string _customgroup = "-1";
        private string _defualtgroup = "-1";
        private string _grade = "-1";
        protected HiddenField txt_CustomGroup;
        protected HiddenField txt_DefualtGroup;
        protected HiddenField txt_Grades;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string CustomGroup
        {
            get
            {
                return this._customgroup;
            }
            set
            {
                this._customgroup = value;
            }
        }

        public string DefualtGroup
        {
            get
            {
                return this._defualtgroup;
            }
            set
            {
                this._defualtgroup = value;
            }
        }

        public string Grade
        {
            get
            {
                return this._grade;
            }
            set
            {
                this._grade = value;
            }
        }
    }
}


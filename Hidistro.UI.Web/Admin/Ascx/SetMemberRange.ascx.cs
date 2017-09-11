using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Ascx
{
    public partial class SetMemberRange : System.Web.UI.UserControl
    {
        private string _customgroup = "-1";
        private string _defualtgroup = "-1";
        private string _grade = "-1";

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
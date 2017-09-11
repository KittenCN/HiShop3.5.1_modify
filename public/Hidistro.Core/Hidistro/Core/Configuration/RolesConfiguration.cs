namespace Hidistro.Core.Configuration
{
    using System;
    using System.Globalization;

    public class RolesConfiguration
    {
        private string manager = "Manager";
        private string member = "Member";
        private string systemAdmin = "SystemAdministrator";

        public string RoleList()
        {
            return string.Format(CultureInfo.InvariantCulture, "^({0}|{1}|{2})$", new object[] { this.Member, this.SystemAdministrator, this.Manager });
        }

        public string Manager
        {
            get
            {
                return this.manager;
            }
        }

        public string Member
        {
            get
            {
                return this.member;
            }
        }

        public string SystemAdministrator
        {
            get
            {
                return this.systemAdmin;
            }
        }
    }
}


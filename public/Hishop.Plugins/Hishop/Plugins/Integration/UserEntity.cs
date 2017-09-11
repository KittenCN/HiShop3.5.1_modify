﻿namespace Hishop.Plugins.Integration
{
    using System;
    using System.Data;

    public class UserEntity
    {
        private string invisible = string.Empty;
        private string pmsound = string.Empty;
        private string ppp = string.Empty;
        private string sigstatus = string.Empty;
        private string tpp = string.Empty;
        private string uid = string.Empty;

        public UserEntity(DataTable uinfo)
        {
            if (!object.Equals(uinfo, null) && (uinfo.Rows.Count == 1))
            {
                this.tpp = uinfo.Rows[0][0].ToString();
                this.ppp = uinfo.Rows[0][1].ToString();
                this.pmsound = uinfo.Rows[0][2].ToString();
                this.invisible = uinfo.Rows[0][3].ToString();
                this.sigstatus = uinfo.Rows[0][4].ToString();
                this.uid = uinfo.Rows[0][5].ToString();
            }
        }

        public string Invisible
        {
            get
            {
                return this.invisible;
            }
        }

        public string Pmsound
        {
            get
            {
                return this.pmsound;
            }
        }

        public string Ppp
        {
            get
            {
                return this.ppp;
            }
        }

        public string Sigstatus
        {
            get
            {
                return this.sigstatus;
            }
        }

        public string Tpp
        {
            get
            {
                return this.tpp;
            }
        }

        public string Uid
        {
            get
            {
                return this.uid;
            }
        }
    }
}


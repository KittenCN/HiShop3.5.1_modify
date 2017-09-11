namespace Hidistro.Core.Json
{
    using System;

    public class JsonAttribute
    {
        private string _Name;
        private object _Value;

        public JsonAttribute(string strName, object strValue)
        {
            this._Name = strName;
            this._Value = strValue;
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }
    }
}


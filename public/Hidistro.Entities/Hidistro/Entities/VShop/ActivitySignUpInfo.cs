namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    public class ActivitySignUpInfo
    {
        public ActivitySignUpInfo()
        {
            this.SignUpDate = DateTime.Now;
        }

        public int ActivityId { get; set; }

        public int ActivitySignUpId { get; set; }

        public string Item1 { get; set; }

        public string Item2 { get; set; }

        public string Item3 { get; set; }

        public string Item4 { get; set; }

        public string Item5 { get; set; }

        public string RealName { get; set; }

        public DateTime SignUpDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}


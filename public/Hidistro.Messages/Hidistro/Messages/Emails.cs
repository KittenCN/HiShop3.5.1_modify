namespace Hidistro.Messages
{
    using Hidistro.Core.Entities;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Net.Mail;

    public static class Emails
    {
        internal static void EnqueuEmail(MailMessage email, SiteSettings settings)
        {
            if (((email != null) && (email.To != null)) && (email.To.Count > 0))
            {
                new EmailQueueDao().QueueEmail(email);
            }
        }
    }
}


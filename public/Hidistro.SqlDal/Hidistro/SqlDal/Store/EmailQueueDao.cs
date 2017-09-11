namespace Hidistro.SqlDal.Store
{
    using Hidistro.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Net.Mail;

    public class EmailQueueDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void DeleteQueuedEmail(Guid emailId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_EmailQueue WHERE EmailId = @EmailId");
            this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public Dictionary<Guid, MailMessage> DequeueEmail()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_EmailQueue WHERE NextTryTime < getdate()");
            Dictionary<Guid, MailMessage> dictionary = new Dictionary<Guid, MailMessage>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    MailMessage message = this.PopulateEmailFromIDataReader(reader);
                    if (message != null)
                    {
                        dictionary.Add((Guid) reader["EmailId"], message);
                    }
                    else
                    {
                        this.DeleteQueuedEmail((Guid) reader["EmailId"]);
                    }
                }
                reader.Close();
            }
            return dictionary;
        }

        public MailMessage PopulateEmailFromIDataReader(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            try
            {
                MailMessage message = new MailMessage {
                    Priority = (MailPriority) ((int) reader["EmailPriority"]),
                    IsBodyHtml = (bool) reader["IsBodyHtml"]
                };
                if (reader["EmailSubject"] != DBNull.Value)
                {
                    message.Subject = (string) reader["EmailSubject"];
                }
                if (reader["EmailTo"] != DBNull.Value)
                {
                    message.To.Add((string) reader["EmailTo"]);
                }
                if (reader["EmailBody"] != DBNull.Value)
                {
                    message.Body = (string) reader["EmailBody"];
                }
                if (reader["EmailCc"] != DBNull.Value)
                {
                    foreach (string str in ((string) reader["EmailCc"]).Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            message.CC.Add(new MailAddress(str));
                        }
                    }
                }
                if (reader["EmailBcc"] != DBNull.Value)
                {
                    foreach (string str2 in ((string) reader["EmailBcc"]).Split(new char[] { ',' }))
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            message.Bcc.Add(new MailAddress(str2));
                        }
                    }
                }
                return message;
            }
            catch
            {
                return null;
            }
        }

        public void QueueEmail(MailMessage message)
        {
            if (message != null)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_EmailQueue(EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
                this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, Guid.NewGuid());
                this.database.AddInParameter(sqlStringCommand, "EmailTo", DbType.String, Globals.ToDelimitedString(message.To, ","));
                if (message.CC != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, Globals.ToDelimitedString(message.CC, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, DBNull.Value);
                }
                if (message.Bcc != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, Globals.ToDelimitedString(message.Bcc, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, DBNull.Value);
                }
                this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, message.Subject);
                this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, message.Body);
                this.database.AddInParameter(sqlStringCommand, "EmailPriority", DbType.Int32, (int) message.Priority);
                this.database.AddInParameter(sqlStringCommand, "IsBodyHtml", DbType.Boolean, message.IsBodyHtml);
                this.database.AddInParameter(sqlStringCommand, "NextTryTime", DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
                this.database.AddInParameter(sqlStringCommand, "NumberOfTries", DbType.Int32, 0);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void QueueSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_EmailQueue_Failure");
            this.database.AddInParameter(storedProcCommand, "EmailId", DbType.Guid);
            this.database.AddInParameter(storedProcCommand, "FailureInterval", DbType.Int32, failureInterval);
            this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", DbType.Int32, maxNumberOfTries);
            foreach (Guid guid in list)
            {
                storedProcCommand.Parameters[0].Value = guid;
                this.database.ExecuteNonQuery(storedProcCommand);
            }
        }
    }
}


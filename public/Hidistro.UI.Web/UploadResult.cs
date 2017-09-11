using System;
using System.Runtime.CompilerServices;

public class UploadResult
{
    public string ErrorMessage { get; set; }

    public string OriginFileName { get; set; }

    public UploadState State { get; set; }

    public string Url { get; set; }
}


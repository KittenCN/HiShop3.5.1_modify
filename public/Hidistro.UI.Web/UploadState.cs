using System;

public enum UploadState
{
    FileAccessError = -3,
    NetworkError = -4,
    SizeLimitExceed = -1,
    Success = 0,
    TypeNotAllow = -2,
    Unknown = 1
}


namespace Hishop.TransferManager
{
    using System;

    public class PPTarget : Target
    {
        public const string TargetName = "拍拍助理";

        public PPTarget(string versionString) : base("拍拍助理", versionString)
        {
        }

        public PPTarget(Version version) : base("拍拍助理", version)
        {
        }

        public PPTarget(int major, int minor, int build) : base("拍拍助理", major, minor, build)
        {
        }
    }
}


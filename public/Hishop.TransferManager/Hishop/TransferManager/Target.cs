namespace Hishop.TransferManager
{
    using System;
    using System.Runtime.CompilerServices;

    public class Target
    {
        public Target(string name, string versionString) : this(name, new System.Version(versionString))
        {
        }

        public Target(string name, System.Version version)
        {
            this.Name = name;
            this.Version = version;
        }

        public Target(string name, int major, int minor, int build) : this(name, new System.Version(major, minor, build))
        {
        }

        public override string ToString()
        {
            return (this.Name + this.Version.ToString());
        }

        public string Name { get; private set; }

        public System.Version Version { get; private set; }
    }
}


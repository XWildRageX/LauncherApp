using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Classes
{
    struct Version
    {
        internal static Version zero = new Version(0,0,0);

        private short major;
        private short minor;
        private short subMinor;

        internal Version(short major, short minor, short subMinor)
        {
            this.major = major;
            this.minor = minor;
            this.subMinor = subMinor;
        }

        internal Version(string version)
        {
            string[] versionString = version.Split(".");
            if(versionString.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }
            major = short.Parse(versionString[0]);
            minor = short.Parse(versionString[1]);
            subMinor = short.Parse(versionString[2]);
        }

        internal bool IsDifferentThan(Version otherVersion)
        {
            if(major != otherVersion.major || minor != otherVersion.minor || subMinor != otherVersion.subMinor) 
            { 
                return true; 
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }
}

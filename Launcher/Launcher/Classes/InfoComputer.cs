using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Classes
{
    public class InfoComputer
    {
        public static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            List<string> result = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);
            switch (ClassItemField)
            {
                case "Capacity":
                    int Capacity = 0;
                    foreach (ManagementObject m in searcher.Get()) Capacity += Convert.ToInt32(Math.Round(Convert.ToDouble(m[ClassItemField]) / 1024 / 1024));
                    result.Add(Capacity.ToString() + " Мб");
                    break;
                default:
                    foreach (ManagementObject obj in searcher.Get()) result.Add(obj[ClassItemField].ToString().Trim());
                    break;
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengerTransportationProject.Classes
{
    public class SpaceCheck
    {
        public static bool Check(string space)
        {
            if (string.IsNullOrWhiteSpace(space) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

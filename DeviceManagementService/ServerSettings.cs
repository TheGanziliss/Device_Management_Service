using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService
{
    internal interface ServerSettings
    {
        public static string ServerIp { get; set; }
        public static string userName { get; set; }
        public static string password { get; set; }
        public static string database { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseIdentityServer
{
    public class GlobalAppSettings
    {
        public static GlobalAppSettings Instance = new GlobalAppSettings();

        public GlobalAppSettings()
        {        
             Instance = this;
        }
    }
}

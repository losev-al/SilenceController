using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    public class KillPaths : ConfigurationSection
    {
        [ConfigurationProperty("Paths")]
        public PathCollection PathItems
        {
            get { return ((PathCollection)(base["Paths"])); }
        }
    }
}

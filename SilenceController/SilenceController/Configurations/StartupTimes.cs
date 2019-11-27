using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    public class StartupTimes : ConfigurationSection
    {
        [ConfigurationProperty("Times")]
        public TimeCollection TimeItems
        {
            get { return ((TimeCollection)(base["Times"])); }
        }
    }
}

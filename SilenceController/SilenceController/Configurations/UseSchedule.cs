using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    public class UseSchedule : ConfigurationElement
    {
        [ConfigurationProperty("Value", DefaultValue = "false", IsKey = false, IsRequired = true)]
        public bool Value
        {
            get { return ((bool)(base["Value"])); }
            set { base["Value"] = value; }

        }
    }
}

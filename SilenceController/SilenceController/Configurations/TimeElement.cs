using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    public class TimeElement : ConfigurationElement
    {
        [ConfigurationProperty("Value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return ((string)(base["Value"])); }
            set { base["Value"] = value; }

        }
    }
}

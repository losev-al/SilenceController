using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    class Pomodoro : ConfigurationSection
    {
        [ConfigurationProperty("WorkInterval", DefaultValue = "00:20:00", IsKey = false, IsRequired = true)]
        public TimeSpan WorkInterval
        {
            get { return ((TimeSpan)(base["WorkInterval"])); }
            set { base["WorkInterval"] = value; }

        }

        [ConfigurationProperty("BreakInterval", DefaultValue = "00:20:00", IsKey = false, IsRequired = true)]
        public TimeSpan BreakInterval
        {
            get { return ((TimeSpan)(base["BreakInterval"])); }
            set { base["BreakInterval"] = value; }

        }

        [ConfigurationProperty("WorkCycles", DefaultValue = "4", IsKey = false, IsRequired = true)]
        public int WorkCycles
        {
            get { return ((int)(base["WorkCycles"])); }
            set { base["WorkInterval"] = value; }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    public class ApplicationMode : ConfigurationSection
    {
        [ConfigurationProperty("UseSchedule")]
        public UseSchedule UseSchedule
        {
            get { return ((UseSchedule)(base["UseSchedule"])); }
        }
    }
}

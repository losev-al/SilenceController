using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    [ConfigurationCollection(typeof(TimeElement), AddItemName = "Time")]
    public class TimeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TimeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TimeElement)element).Value;
        }

        public TimeElement this[int idx]
        {
            get { return (TimeElement)BaseGet(idx); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilenceController.Configurations
{
    [ConfigurationCollection(typeof(PathElement), AddItemName = "Path")]
    public class PathCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)element).Value;
        }

        public PathElement this[int idx]
        {
            get { return (PathElement)BaseGet(idx); }
        }
    }
}

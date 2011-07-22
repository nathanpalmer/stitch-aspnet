using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Stitch.Web
{
    public class StitchConfigurationSection : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var t = typeof(StitchConfiguration);
            var ser = new XmlSerializer(t);

            var config = ser.Deserialize(new XmlNodeReader(section)) as StitchConfiguration;
            if (config != null)
            {
                return config;
            }

            return null;
        }
    }
}
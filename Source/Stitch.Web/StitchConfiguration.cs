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

        [XmlRoot("stitch")]
        public class StitchConfiguration
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlArray("paths")]
            [XmlArrayItem("path")]
            public string[] Paths { get; set; }

            [XmlArray("dependencies")]
            [XmlArrayItem("file")]
            public string[] Dependencies { get; set; }

            [XmlElement("identifier")]
            public string Identifier { get; set; }

            [XmlArray("compilers")]
            [XmlArrayItem("compiler")]
            public StitchConfigurationCompiler[] Compilers { get; set; }

            [XmlArray("files")]
            [XmlArrayItem("file")]
            public StitchConfiguration[] Files { get; set; }
        }

        [XmlRoot("compiler")]
        public class StitchConfigurationCompiler
        {
            [XmlAttribute("type")]
            public string Type { get; set; }

            [XmlAttribute("extension")]
            public string Extension { get; set; }
       }
    }
}
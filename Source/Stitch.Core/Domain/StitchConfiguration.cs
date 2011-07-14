using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Stitch
{
    [XmlRoot("stitch")]
    public class StitchConfiguration : IStitchConfiguration
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
        public IStitchConfigurationCompiler[] Compilers { get; set; }

        [XmlArray("files")]
        [XmlArrayItem("file")]
        public IStitchConfiguration[] Files { get; set; }
    }

        

    [XmlRoot("compiler")]
    public class StitchConfigurationCompiler : IStitchConfigurationCompiler
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("extension")]
        public string Extension { get; set; }
    }
}
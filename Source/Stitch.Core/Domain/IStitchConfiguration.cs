using System.Xml.Serialization;

namespace Stitch
{
    public interface IStitchConfiguration
    {
        string Name { get; set; }
        string[] Paths { get; set; }
        string[] Dependencies { get; set; }
        string Identifier { get; set; }
        IStitchConfigurationCompiler[] Compilers { get; set; }
        IStitchConfiguration[] Files { get; set; }
    }

    public interface IStitchConfigurationCompiler
    {
        string Type { get; set; }
        string Extension { get; set; }
    }
}
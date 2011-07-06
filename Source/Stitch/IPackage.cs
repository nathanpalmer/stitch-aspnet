using System.Collections.Generic;

namespace Stitch
{
    public interface IPackage
    {
        string[] Paths { get; set; }
        string Identifier { get; set; }
        string Root { get; set; }
        List<ICompile> Compilers { get; set; }

        string Compile();
    }
}
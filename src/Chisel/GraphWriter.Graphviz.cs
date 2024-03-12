using System.IO;

namespace Chisel;

internal sealed class GraphvizWriter : GraphWriter
{
    public GraphvizWriter(TextWriter writer) : base(writer)
    {
    }

    protected override void WriteHeader(GraphOptions options)
    {
        Writer.WriteLine("# Generated by https://github.com/0xced/Chisel");
        Writer.WriteLine();
        Writer.WriteLine("digraph");
        Writer.WriteLine("{");

        if (options.Direction == GraphDirection.LeftToRight)
            Writer.WriteLine("  rankdir=LR");
        else if (options.Direction == GraphDirection.TopToBottom)
            Writer.WriteLine("  rankdir=TB");

        Writer.WriteLine("  node [ fontname = \"Segoe UI, sans-serif\", shape = box, style = filled, color = aquamarine ]");
        Writer.WriteLine();
    }

    protected override void WriteFooter()
    {
        Writer.WriteLine("}");
    }

    protected override void WriteNode(Package package, GraphOptions options)
    {
        Writer.Write($"  \"{GetPackageId(package, options)}\"");
        if (package.State == PackageState.Ignore)
        {
            Writer.Write(" [ color = lightgray ]");
        }
        else if (package.State == PackageState.Remove)
        {
            Writer.Write(" [ color = lightcoral ]");
        }
        else if (package.Type == PackageType.Project)
        {
            Writer.Write(" [ color = skyblue ]");
        }
        else if (package.Type == PackageType.Unknown)
        {
            Writer.Write(" [ color = khaki ]");
        }

        Writer.WriteLine();
    }

    protected override void WriteEdge(Package package, Package dependency, GraphOptions options)
    {
        Writer.WriteLine($"  \"{GetPackageId(package, options)}\" -> \"{GetPackageId(dependency, options)}\"");
    }
}
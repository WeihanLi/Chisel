using System.IO;

namespace Chisel;

internal sealed class GraphvizWriter(TextWriter writer) : GraphWriter(writer)
{
    public override string FormatName => "Graphviz";

    protected override void WriteHeader(bool hasProject, bool hasIgnored, bool hasRemoved, GraphOptions options)
    {
        Writer.WriteLine("# Generated by https://github.com/0xced/Chisel");
        Writer.WriteLine();
        Writer.WriteLine("digraph");
        Writer.WriteLine("{");

        if (options.Direction == GraphDirection.LeftToRight)
            Writer.WriteLine("  rankdir=LR");
        else if (options.Direction == GraphDirection.TopToBottom)
            Writer.WriteLine("  rankdir=TB");

        Writer.WriteLine($"  node [ fontname = \"Segoe UI, sans-serif\", shape = box, style = filled, {Color(options.Color.Default)} ]");
        Writer.WriteLine();
    }

    protected override void WriteFooter()
    {
        Writer.WriteLine("}");
    }

    protected override void WriteRoot(Package package, GraphOptions options)
    {
    }

    protected override void WriteNode(Package package, GraphOptions options)
    {
        Writer.Write($"  \"{GetPackageId(package, options)}\"");
        var color = package.State switch
        {
            PackageState.Ignore => options.Color.Ignored,
            PackageState.Remove => options.Color.Removed,
            _ => package.IsProjectReference ? options.Color.Project : (Color?)null,
        };
        if (color.HasValue)
        {
            Writer.Write($" [ {Color(color.Value)} ]");
        }
        Writer.WriteLine();
    }

    protected override void WriteEdge(Package package, Package dependency, GraphOptions options)
    {
        Writer.WriteLine($"  \"{GetPackageId(package, options)}\" -> \"{GetPackageId(dependency, options)}\"");
    }

    private static string Color(Color color) => $"fillcolor = {Fill(color)}, color = {Stroke(color)}";

    private static string Fill(Color color) => color.Fill.StartsWith("#") ? $"\"{color.Fill}\"" : color.Fill;

    private static string Stroke(Color color) => color.Stroke.StartsWith("#") ? $"\"{color.Stroke}\"" : color.Stroke;
}
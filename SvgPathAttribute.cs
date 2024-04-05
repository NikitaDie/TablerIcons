namespace TablerIcons;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class SvgPathAttribute : Attribute
{
    public SvgPathAttribute(int width, int height, string path)
    {
        this.Width = width;
        this.Height = height;
        this.Path = path;
    }

    public int Width { get; }

    public int Height { get; }

    public string Path { get; }
}
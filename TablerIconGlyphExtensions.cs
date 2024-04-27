using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;

namespace TablerIcons;

public static class TablerIconGlyphExtensions
{
    public static Path? CreatePath(this TablerIconGlyph glyph, Brush foregroundBrush, ushort thickness) 
    {
        string path1;
        int width;
        int height;
        if (!glyph.GetSvg(out path1, out width, out height))
            return (Path) null;
        Path path2 = new Path();
        path2.Data = Geometry.Parse(path1);
        path2.StrokeThickness = thickness;
        path2.StrokeLineJoin = PenLineJoin.Round;
        path2.StrokeStartLineCap = PenLineCap.Round;
        path2.StrokeEndLineCap = PenLineCap.Round;
        path2.Stroke = foregroundBrush;
        path2.Width = (double) width;
        path2.Height = (double) height;
        // path2.Fill = foregroundBrush;
        return path2;
    }
    
    public static Path? CreatePath(this TablerIconGlyph glyph, Brush foregroundBrush) 
    {
        string path1;
        int width;
        int height;
        if (!glyph.GetSvg(out path1, out width, out height))
            return (Path) null;
        Path path2 = new Path();
        path2.Data = Geometry.Parse(path1);
        path2.StrokeThickness = 0;
        // path2.StrokeLineJoin = PenLineJoin.Round;
        // path2.StrokeStartLineCap = PenLineCap.Round;
        // path2.StrokeEndLineCap = PenLineCap.Round;
        path2.Stroke = foregroundBrush;
        path2.Width = (double) width;
        path2.Height = (double) height;
        path2.Fill = foregroundBrush;
        return path2;
    }
    
    public static bool GetSvg(this TablerIconGlyph glyph, out string? path, out int width, out int height)
    {
        path = (string) null;
        width = -1;
        height = -1;
        TypeInfo enumType = typeof (TablerIconGlyph).GetTypeInfo();
        MemberInfo element = ((IEnumerable<MemberInfo>) enumType.GetMember(glyph.ToString())).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (m => m.DeclaringType == enumType.AsType()));
        SvgPathAttribute customAttribute = (object) element != null ? element.GetCustomAttribute<SvgPathAttribute>() : (SvgPathAttribute) null;
        if (customAttribute == null)
            return false;
        path = customAttribute.Path;
        width = customAttribute.Width;
        height = customAttribute.Height;
        return true;
    }
}
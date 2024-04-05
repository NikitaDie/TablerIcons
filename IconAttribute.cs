namespace TablerIcons;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class IconIdAttribute : Attribute
{
    public IconIdAttribute(string iconId) => this.IconId = iconId;

    public string IconId { get; }
}
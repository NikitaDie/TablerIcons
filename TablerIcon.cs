using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TablerIcons;

public partial class TablerIcon : Viewbox
{
    private static Regex _regexToFindFilled = MyRegex();
    
    [GeneratedRegex(@"Filled$")]
    private static partial Regex MyRegex();
    
    public static readonly DependencyProperty ForegroundProperty = 
        DependencyProperty.Register(nameof (Foreground), typeof (Brush), typeof (TablerIcon), new PropertyMetadata(Brushes.Black, 
            OnIconPropertyChanged));
    public static readonly DependencyProperty IconProperty = 
        DependencyProperty.Register(nameof (Icon), typeof (TablerIconGlyph), typeof (TablerIcon), 
            new PropertyMetadata(TablerIconGlyph.None, OnIconPropertyChanged));
    public static readonly DependencyProperty ThicknessProperty = 
        DependencyProperty.Register(nameof (Thickness), typeof (ushort), typeof (TablerIcon), 
            new PropertyMetadata((ushort) 2, OnIconPropertyChanged));
    public static readonly DependencyProperty RotationProperty = 
        DependencyProperty.Register(nameof (Rotation), typeof (double), typeof (TablerIcon), 
            new PropertyMetadata(0.0, RotationChanged, RotationCoerceValue));
    public static readonly DependencyProperty FlipProperty = 
        DependencyProperty.Register(nameof (Flip), typeof (FlipOrientation), typeof (TablerIcon), 
            new PropertyMetadata((object) FlipOrientation.Normal, new PropertyChangedCallback(FlipChanged)));

    static TablerIcon()
    {
        UIElement.OpacityProperty.OverrideMetadata(typeof (TablerIcon), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));
    }

    public TablerIcon() { }
    
    public Brush Foreground
    {
        get => (Brush) GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public TablerIconGlyph Icon
    {
        get => (TablerIconGlyph) this.GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ushort Thickness
    {
        get => (ushort) GetValue(ThicknessProperty);
        set => this.SetValue(ThicknessProperty, value);
    }
    
    private static void OnIconPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (!(d is TablerIcon tablericon))
            return;

        var iconName = Enum.GetName(typeof(TablerIconGlyph), tablericon.Icon);
        
        if (_regexToFindFilled.IsMatch(iconName))
        {
            tablericon.Child = tablericon.Icon != TablerIconGlyph.None ? 
                (UIElement) tablericon.Icon.CreatePath(tablericon.Foreground) : (UIElement) null;
        }
        else
        {
            tablericon.Child = tablericon.Icon != TablerIconGlyph.None ? 
                (UIElement) tablericon.Icon.CreatePath(tablericon.Foreground, tablericon.Thickness) : (UIElement) null; 
        }
    }
    
    public double Rotation
    {
        get => (double) this.GetValue(RotationProperty);
        set => this.SetValue(RotationProperty, (object) value);
    }

    private static void RotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is TablerIcon tablerIcon) || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue))
            return;
        tablerIcon.SetRotation();
    }

    private static object RotationCoerceValue(DependencyObject d, object value)
    {
        double num = (double) value;
        while (num < 0.0)
            num += 360.0;
        while (num >= 360.0)
            num -= 360.0;
        return (object) num;
    }

    public FlipOrientation Flip
    {
        get => (FlipOrientation) this.GetValue(FlipProperty);
        set => this.SetValue(FlipProperty, (object) value);
    }

    private static void FlipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is TablerIcon tablerIconIcon) || !(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue))
            return;
        tablerIconIcon.SetFlip();
    }

    private void SetRotation()
    {
        if (!(this.RenderTransform is TransformGroup transformGroup1))
            transformGroup1 = new TransformGroup();
        TransformGroup transformGroup2 = transformGroup1;
        RotateTransform rotateTransform = transformGroup2.Children.OfType<RotateTransform>().FirstOrDefault<RotateTransform>();
        if (rotateTransform != null)
        {
            rotateTransform.Angle = this.Rotation;
        }
        else
        {
            transformGroup2.Children.Add((Transform) new RotateTransform()
            {
                Angle = this.Rotation
            });
            this.RenderTransform = (Transform) transformGroup2;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }
    }

    private void SetFlip()
    {
        if (!(this.RenderTransform is TransformGroup transformGroup1))
            transformGroup1 = new TransformGroup();
        TransformGroup transformGroup2 = transformGroup1;
        int num1 = this.Flip.HasFlag((Enum) FlipOrientation.Horizontal) ? -1 : 1;
        int num2 = this.Flip.HasFlag((Enum) FlipOrientation.Vertical) ? -1 : 1;
        ScaleTransform scaleTransform = transformGroup2.Children.OfType<ScaleTransform>().FirstOrDefault<ScaleTransform>();
        if (scaleTransform != null)
        {
            scaleTransform.ScaleX = (double) num1;
            scaleTransform.ScaleY = (double) num2;
        }
        else
        {
            transformGroup2.Children.Add((Transform) new ScaleTransform()
            {
                ScaleX = (double) num1,
                ScaleY = (double) num2
            });
            this.RenderTransform = (Transform) transformGroup2;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }
    }
}
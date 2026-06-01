using System.Windows;
using System.Windows.Controls;

namespace SoundEffect.Lib.Controls
{
    internal static class Spacing
    {
        public static readonly DependencyProperty HorizontalProperty =
            DependencyProperty.RegisterAttached("Horizontal", typeof(double), typeof(Spacing),
                new PropertyMetadata(0.0, OnHorizontalChanged));

        public static double GetHorizontal(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalProperty);
        }

        public static void SetHorizontal(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalProperty, value);
        }

        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StackPanel panel)
            {
                panel.Loaded += (s, args) => ApplySpacing(panel);
            }
        }

        private static void ApplySpacing(StackPanel panel)
        {
            double spacing = GetHorizontal(panel);
            for (int i = 0; i < panel.Children.Count - 1; i++)
            {
                if (panel.Children[i] is FrameworkElement element)
                    element.Margin = new Thickness(0, 0, spacing, 0);
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundEffect.Lib.Controls
{
    public partial class TextBlockEdit : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockEdit), new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsEditing
        {
            get { return PART_TextBox.Visibility == Visibility.Visible; }
            set
            {
                PART_TextBlock.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                PART_TextBox.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (value)
                {
                    PART_TextBox.Focus();
                    PART_TextBox.SelectAll();
                }
            }
        }

        public TextBlockEdit()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsEditing = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditing = false;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                IsEditing = false;
        }
    }
}

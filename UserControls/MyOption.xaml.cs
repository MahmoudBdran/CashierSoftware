using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CashierSoftware.UserControls
{
    public partial class MyOption : UserControl
    {
        public MyOption()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MyOption));

        public ImageSource ImageIcon
        {
            get { return (ImageSource)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }
        public static readonly DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageIcon", typeof(ImageSource), typeof(MyOption));

        public MahApps.Metro.IconPacks.PackIconMaterial Icon
        {
            get { return (MahApps.Metro.IconPacks.PackIconMaterial)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(MahApps.Metro.IconPacks.PackIconMaterial), typeof(MyOption));

    }
}
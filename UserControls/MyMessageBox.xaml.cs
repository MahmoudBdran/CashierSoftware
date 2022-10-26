using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashierSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : UserControl
    {
        public MyMessageBox()
        {
            InitializeComponent();
        }
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(MyMessageBox));


        private void CloseIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }
    }
}

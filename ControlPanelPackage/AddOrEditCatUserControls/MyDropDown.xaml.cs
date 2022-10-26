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

namespace CashierSoftware.ControlPanelPackage.AddOrEditCatUserControls
{
    /// <summary>
    /// Interaction logic for MyDropDown.xaml
    /// </summary>
    public partial class MyDropDown : UserControl
    {
        public MyDropDown()
        {
            InitializeComponent();
        }
        /*
public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(MyDropDown));

        public string CustomText
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }
        public static DependencyProperty CustomTextProperty = DependencyProperty.Register("CustomText", typeof(string), typeof(MyDropDown));





        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(MyDropDown));

        private void dropdown_TextInput(object sender, TextCompositionEventArgs e)
        {
           
            MainCatDropDown.IsDropDownOpen = true;
            MainCatDropDown.ItemsSource = MainCatDropDown.ItemsSource.ToString().Where(p => p.Name.Contains(e.Text)).ToList();
        }         */
    }
}
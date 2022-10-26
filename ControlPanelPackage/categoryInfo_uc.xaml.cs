using CashierSoftware.ControlPanelPackage.addorMainCatName;
using CashierSoftware.ControlPanelPackage.EditCatInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CashierSoftware
{
    /// <summary>
    /// Interaction logic for categoryInfoClass.xaml
    /// </summary>
    public partial class categoryInfoClass : UserControl
    {

        DataTable dt = new DataTable();
        public categoryInfoClass()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            //Create DataGrid items info
            loadDataGridFromSql();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
              
            }
        }
        void loadDataGridFromSql() {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * from category", con);
            cmd.Fill(dt);
            membersDataGrid.ItemsSource = dt.DefaultView;
            membersDataGrid.Columns[0].Header = "السريال";
            membersDataGrid.Columns[1].Header = "اسم المنتج";
            membersDataGrid.Columns[2].Header = "السعر الأصلي";
            membersDataGrid.Columns[3].Header = "الخصم";
            membersDataGrid.Columns[4].Header = "السعر النهائى";
            membersDataGrid.Columns[5].Header = "الكمية";
            membersDataGrid.Columns[6].Header = "فئة";

            con.Close();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void minimize_btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * from category", con);
            DataTable dt = new DataTable();
            cmd.Fill(dt);
            membersDataGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");


        private void DG_edit_btn_Click(object sender, RoutedEventArgs e)
        {
            testText.Text = membersDataGrid.SelectedIndex.ToString();
            string id = dt.Rows[membersDataGrid.SelectedIndex]["id"].ToString();
            EditCatInfoWindow editCatInfoWindow = new EditCatInfoWindow("edit",id);
            try
            {
                this.IsEnabled = false;
                editCatInfoWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.IsEnabled = true;
            }

        }

        private void DG_remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CatAddBtn_Click(object sender, RoutedEventArgs e)
        {
            EditCatInfoWindow editCatInfoWindow = new EditCatInfoWindow("insert","0");
            try
            {
                this.IsEnabled = false;
                editCatInfoWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

                this.IsEnabled = true;
            }
        }

        private void MainCatAddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditMainCategory mainCategory = new AddOrEditMainCategory();
            mainCategory.ShowDialog();
        }
    }
   
}

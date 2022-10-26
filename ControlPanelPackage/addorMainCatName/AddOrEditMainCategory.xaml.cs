using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
using System.Windows.Shapes;
using System.Windows.Shell;

namespace CashierSoftware.ControlPanelPackage.addorMainCatName
{
    /// <summary>
    /// Interaction logic for AddOrEditMainCategory.xaml
    /// </summary>
    public partial class AddOrEditMainCategory : Window
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable dt = new DataTable();
        public AddOrEditMainCategory()
        {
            InitializeComponent(); 
            SaveMainCatNewNameBtn.IsEnabled = false;

            // يتم فك هذا الكومنت عند الرجوع للتعديل
            /*NewMainCattb.Visibility = Visibility.Hidden;
            EditMainCatNameBtn.Visibility = Visibility.Hidden;
            id_tb.Visibility = Visibility.Hidden;
             */
        }



        private void btn_grid_edit_Click(object sender, RoutedEventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            NewMainCattb.Visibility = Visibility.Visible;
            EditMainCatNameBtn.Visibility = Visibility.Visible;
            id_tb.Visibility = Visibility.Visible;
            SqlCommand cmd = new SqlCommand("select MainCatName from MainCategory where id =@id",con);
            cmd.Parameters.AddWithValue("@id", dt.Rows[MainCatDG.SelectedIndex]["id"]);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                id_tb.CustomText = dt.Rows[MainCatDG.SelectedIndex]["id"].ToString();
                EditMainCattb.CustomText = dr[0].ToString();
                dr.Close();
            }

        }

     

        private void deleteRow_btn_Click_1(object sender, RoutedEventArgs e)
        {
            /*if (MessageBox.Show("هل تريد حذف هذه الفئة؟", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {


            }
            else
            {
                dt.Rows[MainCatDG.SelectedIndex].Delete();

                dt.AcceptChanges();

            }
             */
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditMainCattb_LostFocus(object sender, RoutedEventArgs e)
        {
           
            
                NewMainCattb.Visibility = Visibility.Hidden;
                EditMainCatNameBtn.Visibility = Visibility.Hidden;
                id_tb.Visibility = Visibility.Hidden;
                NewMainCattb.CustomText = "";
                id_tb.CustomText = "";
            
        }

        private void EditMainCatNameBtn_Click(object sender, RoutedEventArgs e)
        {
            /* if (MessageBox.Show("هل تريد تعديل اسم هذه الفئة؟", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
             {
                 NewMainCattb.Visibility = Visibility.Hidden;
                 EditMainCatNameBtn.Visibility = Visibility.Hidden;
                 id_tb.Visibility = Visibility.Hidden;
                 NewMainCattb.CustomText = "";
                 id_tb.CustomText = "";

             }
             else
             {
                 try
                 {
                     if (con.State == ConnectionState.Closed)
                         con.Open(); 
                     SqlCommand cmd = new SqlCommand("update MainCategory set MainCatName= @MainCatName where id = @id", con);
                     cmd.Parameters.AddWithValue("@MainCatName", EditMainCattb.CustomText);
                     cmd.Parameters.AddWithValue("@id", int.Parse(id_tb.CustomText));

                     MessageBox.Show("تم تحديث اسم الفئة بنجاح");
                 }
                 catch(Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }

             }
             */
        }

        private void SaveMainCatNewNameBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("insert into MainCategory(MainCatName) values (@MainCatName)", con);
                cmd.Parameters.AddWithValue("@MainCatName", NewMainCattb.CustomText);

                cmd.ExecuteNonQuery();
                MessageBox.Show("تم حفظ اسم الفئة بنجاح");
                NewMainCattb.CustomText = "";
                realoadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void realoadDataGrid()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * from MainCategory", con);

            cmd.Fill(dt);
            MainCatDG.ItemsSource = dt.DefaultView;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            realoadDataGrid();

        }

        private void NewMainCattb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewMainCattb.CustomText.Length > 0)
            {
                SaveMainCatNewNameBtn.IsEnabled = true;
            }
            else
            {
                SaveMainCatNewNameBtn.IsEnabled = false;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

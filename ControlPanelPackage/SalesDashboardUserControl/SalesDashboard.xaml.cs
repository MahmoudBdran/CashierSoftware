using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace CashierSoftware.ControlPanelPackage.SalesDashboardUserControl
{
    /// <summary>
    /// Interaction logic for SalesDashboard.xaml
    /// </summary>
    public partial class SalesDashboard : UserControl
    {
       
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");

        private static BackgroundWorker backgroundWorker;
        public SalesDashboard()
        {
            InitializeComponent();
            SelecstedDate.SelectedDate =Convert.ToDateTime("24/10/2022");
            SelecstedDate.SelectedDateFormat = DatePickerFormat.Short;
            cat_data_rb.IsChecked = true;
            all_data_rb.IsChecked = false;
            
            
        }

        private void SalesDashboard_Loaded(object sender, RoutedEventArgs e)
        {
           
           // retrieveSqlDB();
        }

        private void BillSearchbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        void retrieveSqlDB() {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("select * from (select row_number() over(order by CONVERT (date, saleDate ,103) desc) AS num_row , CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,CONVERT (date, saleDate ,103) as saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber) as sales where num_row between 1 and 80 ORDER BY saleDate desc, saleTime desc;", con);
                // SqlDataAdapter cmd = new SqlDataAdapter("select CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,CONVERT (date, saleDate ,103) as saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber ORDER BY saleDate desc, saleTime desc", con);
               
                /* SqlDataAdapter cmd = new SqlDataAdapter("select CatSerial,CatName,CatQuantity,BillNumber," +
                     "Price,saleTime,saleDate,BasePrice,Discount,FinalPrice,CustomerPay," +
                     "Rest,Merchant from SalesHistory sh join sellingBillInfo sb" +
                     " on sh.BillNumber = sb.SellingBillnumber ", con);*/
                DataTable dt = new DataTable();
                cmd.Fill(dt);
                SalesDashboardDG.ItemsSource = dt.DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

            }
        }

        private void SelectedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (all_data_rb.IsChecked==true)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber where saleDate = @saleDate ORDER BY saleDate desc, saleTime desc", con);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@saleDate", SelecstedDate.Text);

                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);

                    SalesDashboardDG.ItemsSource = dt.DefaultView;

                    con.Close();
                    SalesDashboardDG.Columns[0].Header = "السريال";
                    SalesDashboardDG.Columns[1].Header = "اسم المنتج";
                    SalesDashboardDG.Columns[2].Header = "الكمية";
                    SalesDashboardDG.Columns[3].Header = "رقم الفاتورة";
                    SalesDashboardDG.Columns[4].Header = "قيمة المنتج";
                    SalesDashboardDG.Columns[5].Header = "وقت";
                    SalesDashboardDG.Columns[6].Header = "تاريخ";
                    SalesDashboardDG.Columns[7].Header = "السعر الأصلى";
                    SalesDashboardDG.Columns[8].Header = "خصومات";
                    SalesDashboardDG.Columns[9].Header = "السعر النهائي";
                    SalesDashboardDG.Columns[10].Header = "دفع العميل";
                    SalesDashboardDG.Columns[11].Header = "الباقى";
                    SalesDashboardDG.Columns[12].Header = "كاشير";
                    calculatePrices();
                    /*<DataGridTextColumn Header="السريال" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding CatSerial}"/>
                    <DataGridTextColumn Header="المنتج" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding CatName}"/>
                    <DataGridTextColumn Header="الكمية" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding CatQuantity}"/>
                    <DataGridTextColumn Header="رقم الفاتورة" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding BillNumber}"/>
                    <DataGridTextColumn Header="قيمة المنتج" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding Price}"/>
                    <DataGridTextColumn Header="وقت" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding saleTime}"/>
                    <DataGridTextColumn Header="تاريخ" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding saleDate}"/>
                    <DataGridTextColumn Header="السعر الأصلى" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding BasePrice}"/>
                    <DataGridTextColumn Header="خصومات" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding Discount}"/>
                    <DataGridTextColumn Header="السعر النهائي" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding FinalPrice}"/>
                    <DataGridTextColumn Header="دفع العميل" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding CustomerPay}"/>
                    <DataGridTextColumn Header="الباقى" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding Rest}"/>
                    <DataGridTextColumn Header="كاشير" IsReadOnly="True" CanUserResize="False" Width="*" Binding="{Binding Merchant}"/>
                     */
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("1 : "+ex.Message);
                }
            }
            else if (cat_data_rb.IsChecked == true)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select CatSerial,CatName,Sum(CONVERT(int,CatQuantity)) as CatQuantity , Sum(CONVERT(int,Price) * CONVERT(int,CatQuantity)) as Price from SalesHistory where saleDate = @saleDate group by CatName , CatSerial ", con);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@saleDate", SelecstedDate.Text);


                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);
                    SalesDashboardDG.ItemsSource = dt.DefaultView;
                    con.Close();
                    SalesDashboardDG.Columns[0].Header = "السريال";
                    SalesDashboardDG.Columns[1].Header = "اسم المنتج";
                    SalesDashboardDG.Columns[2].Header = "الكمية";
                    SalesDashboardDG.Columns[3].Header = "السعر";
                    calculateCatPrices(dt);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("2 : " + ex.Message);
                }
            }
        }

        private void calculatePrices()
        {
            float baseprice = 0;
            float discount = 0;
            float finalprice = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select BasePrice, Discount, FinalPrice from sellingBillInfo where sellDate = @sellDate ", con);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@sellDate", SelecstedDate.Text);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    baseprice += float.Parse(dt.Rows[i]["BasePrice"].ToString());
                    discount += float.Parse(dt.Rows[i]["Discount"].ToString());
                    finalprice += float.Parse(dt.Rows[i]["FinalPrice"].ToString());
                }
                baseprice_tblock.Text = "السعر الأساسى";
                BasePrice_tb.Text = baseprice.ToString();
                discount_tblock.Text = "الخصومات";
                discount_tb.Text = discount.ToString();
                FinalPrice_grid.Visibility = Visibility.Visible;
                FinalPrice_tb.Text = finalprice.ToString();


                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("3 : " + ex.Message);
            }
        }
        private void calculateCatPrices(DataTable dt)
        {
            float baseprice = 0;
            float CatQuantity = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    baseprice += float.Parse(dt.Rows[i]["Price"].ToString());
                    CatQuantity += float.Parse(dt.Rows[i]["CatQuantity"].ToString());
                }
                BasePrice_tb.Text = baseprice.ToString();
                baseprice_tblock.Text = "داخل الخزينة";
                //CatQuantity will be viewed in discount textbox
                discount_tb.Text = CatQuantity.ToString();
                discount_tblock.Text = "الكمية المباعة";
                FinalPrice_grid.Visibility = Visibility.Collapsed;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("3 : " + ex.Message);
            }
        }

        private void cat_data_rb_Checked(object sender, RoutedEventArgs e)
        {
        
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select CatSerial,CatName,Sum(CONVERT(int,CatQuantity)) as CatQuantity , Sum(CONVERT(int,Price) * CONVERT(int,CatQuantity)) as Price from SalesHistory where saleDate = @saleDate group by CatName , CatSerial ", con);
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@saleDate", SelecstedDate.Text);
              

                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                SalesDashboardDG.ItemsSource = dt.DefaultView;
                SalesDashboardDG.Columns[0].Header = "السريال";
                SalesDashboardDG.Columns[1].Header = "اسم المنتج";
                SalesDashboardDG.Columns[2].Header = "الكمية";
                SalesDashboardDG.Columns[3].Header = "السعر";
                con.Close();
                calculateCatPrices(dt);
            }
            catch (Exception ex)
            {
               // MessageBox.Show("4 : " + ex.Message);

            }
        }

        private void all_data_rb_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber where saleDate = @saleDate ORDER BY saleDate desc, saleTime desc", con);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@saleDate", SelecstedDate.Text);


                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);
                SalesDashboardDG.AlternationCount = 13;

                SalesDashboardDG.ItemsSource = dt.DefaultView;
                SalesDashboardDG.Columns[0].Header = "السريال";
                SalesDashboardDG.Columns[1].Header = "اسم المنتج";
                SalesDashboardDG.Columns[2].Header = "الكمية";
                SalesDashboardDG.Columns[3].Header = "رقم الفاتورة";
                SalesDashboardDG.Columns[4].Header = "قيمة المنتج";
                SalesDashboardDG.Columns[5].Header = "وقت";
                SalesDashboardDG.Columns[6].Header = "تاريخ";
                SalesDashboardDG.Columns[7].Header = "السعر الأصلى";
                SalesDashboardDG.Columns[8].Header = "خصومات";
                SalesDashboardDG.Columns[9].Header = "السعر النهائي";
                SalesDashboardDG.Columns[10].Header = "دفع العميل";
                SalesDashboardDG.Columns[11].Header = "الباقى";
                SalesDashboardDG.Columns[12].Header = "كاشير";
                con.Close();
                calculatePrices();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("5 : " + ex.Message);
            }
        }
    }
}

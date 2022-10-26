using BarcodeLib;
using IronBarCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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

namespace CashierSoftware.SellScreenPackage.printPackage
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        DataTable printed_dt;
        String merchant_name;
        String date;
        double total_quantity;
        double customer_pay;
        double total_discount;
        double final_price;
        double rest;
        String billNumber;
        BitmapImage? bitmap = new BitmapImage();

        public PrintWindow(DataTable cat_dt , String merchant_name, String date, double total_quantity, double customer_pay, double total_discount, double final_price, double rest,String billNumber)
        {
            InitializeComponent();
            this.printed_dt = cat_dt;
            preparingPrintedDT();
            this.merchant_name = merchant_name;
            this.date = date;
            this.total_quantity = total_quantity;
            this.customer_pay = customer_pay;
            this.total_discount = total_discount;
            this.final_price = final_price;
            this.rest = rest;
            this.billNumber = billNumber;
            total_quantity_tb.Text = total_quantity.ToString();
            customer_pay_tb.Text = customer_pay.ToString();
            total_discount_tb.Text = total_discount.ToString();
            final_price_tb.Text = final_price.ToString();
            rest_tb.Text = rest.ToString();
            date_tb.Text = date;
            billNumber_tb.Text = billNumber;

            var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(billNumber, BarcodeEncoding.Code93);
            MyBarCode.SaveAsImage(billNumber+".png");
            
            //barcode.Source = (new ImageSourceConverter()).ConvertFromString("BillBarCode.png") as ImageSource;
            //\bin\Debug\net6.0-windows\MyBarCode.png
             
            bitmap.BeginInit(); 
            bitmap.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar+billNumber+".png");
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; 
            bitmap.EndInit();
            
            barcode.Source = bitmap;
        }
        void preparingPrintedDT() {


            //printed_dt columns catName 0,catActualPrice 1,catDiscount 2,quantity 3 ,result  4
            //dt catSerialNumber 0,catName 1,catActualPrice 2,catDiscount 3,catFinalPrice 4,quantity 5
            printed_dt.Columns.Remove("catSerialNumber");

            printed_dt.Columns.Add("result");


            for (int i=0; i < printed_dt.Rows.Count; i++)
            {
                printed_dt.Rows[i]["result"] = (double.Parse(printed_dt.Rows[i]["catFinalPrice"].ToString()) * double.Parse(printed_dt.Rows[i]["quantity"].ToString())).ToString();
            }

            printed_dt.Columns.Remove("catFinalPrice");

            fillRecieptDGInfo();

        }
        void fillRecieptDGInfo()
        {
            printedCategoriesDG.ItemsSource = printed_dt.DefaultView;
            printedCategoriesDG.Columns[0].Header = "الصنف";
            printedCategoriesDG.Columns[1].Header = "السعر";
            printedCategoriesDG.Columns[2].Header = "الخصومات";
            printedCategoriesDG.Columns[3].Header = "الكمية";
            printedCategoriesDG.Columns[4].Header = "القيمة";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == true)
                {

                    printDialog.PrintVisual(print, "invoice");

                }
            }
            finally
            {
                this.IsEnabled = true;
                this.Close();

            }
        }

        private void TextBlock_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }


        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //bitmap.CacheOption =BitmapCacheOption.None;
                //barcode.Source = null;
                File.Delete(System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + billNumber+".png");
                MessageBox.Show("deleted successfully");
                SellScreen sellScreen = new SellScreen(merchant_name);
                sellScreen.Show();
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

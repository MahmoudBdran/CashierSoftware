using CashierSoftware;
using CashierSoftware.ControlPanelPackage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using MahApps.Metro.IconPacks;
using System.Globalization;
using System.Drawing;
using Color = System.Drawing.Color;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CashierSoftware.SellScreenPackage.printPackage;
using IronBarCode;
using System.IO;
using SixLabors.ImageSharp.Drawing;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Threading;
using System.DirectoryServices.ActiveDirectory;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
// عايز ابقي اجرب حتة اني اعمل تايمر بعد كل حرف لو زاد عن 1 ثانية مثلا يروح مدخل البضاعة تلقائي

namespace CashierSoftware.SellScreenPackage
{
    /// <summary>
    /// Interaction logic for SellScreen.xaml
    /// </summary>
    public partial class SellScreen : Window
    {
        DispatcherTimer timer;
        int waitingSecs = 1;
        bool scanningAvailability = true;
        double quant = 0;
        int currentselectedindex = 0;
        private DataGridCellInfo activeCellAtEdit { get; set; }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable categorydt = new DataTable();
        int billnumber = 00;
        bool itemfound = false;
        string merchant;
        public SellScreen(string merchant)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0,0,500);   // half of second = 500 millisecond 
            timer.Tick += Timer_Tick;
            this.Focusable = true;
            this.merchant = merchant;
            CashierName.Text = merchant;
           SaveAndPrint_btn.IsEnabled = false;
            this.Focusable = true;
            mainborder.Focusable = true;
            //  this.corewin

            var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode("https://ironsoftware.com/csharp/barcode", BarcodeEncoding.Code128);
            
        }
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(2),
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (waitingSecs > 0)
            {
                waitingSecs--;
            }
            else
            {
                addingCategory(categoryAdd_tb.Text);
                categoryAdd_tb.Text = "";
                timer.Stop();
            }
        }

        // Methods
        //preparing the saving process
        public DataTable preparingSaveProcess()
        {
            DataTable finalDT = new DataTable();
            finalDT.Columns.Add("CatSerial");
            finalDT.Columns.Add("CatName");
            finalDT.Columns.Add("CatQuantity");
            finalDT.Columns.Add("BillNumber");
            finalDT.Columns.Add("Price");
            finalDT.Columns.Add("Date");
            finalDT.Columns.Add("Time");
            finalDT.Columns.Add("Merchant");
            DataRow dr = null;
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                dr = finalDT.NewRow();
                dr["CatSerial"] = categorydt.Rows[i]["catSerialNumber"].ToString();
                dr["CatName"] = categorydt.Rows[i]["CatName"].ToString();
                dr["CatQuantity"] = categorydt.Rows[i]["quantity"].ToString();
                dr["BillNumber"] = billNumber_tb.Text;
                dr["Price"] = categorydt.Rows[i]["catFinalPrice"].ToString();
                dr["Date"] = DateTime.Now.ToString("dd/MM/yyyy");
                dr["Time"] = DateTime.Now.ToString("HH:mm");
                dr["Merchant"] = merchant;
                finalDT.Rows.Add(dr);

            }

            return finalDT;


        }
        void generateBillNumber()
        {

            while (true)
            {
                billnumber = generateRandomNumber();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand Checkcmd = new SqlCommand("SELECT SellingBillnumber from sellingBillInfo where SellingBillnumber = @billnumber ", con);
                Checkcmd.Parameters.AddWithValue("@billnumber", billnumber);
                SqlDataReader dr = Checkcmd.ExecuteReader();
                if (!dr.Read())
                {
                    billNumber_tb.Text = billnumber.ToString();
                    dr.Close();


                    break;

                }
            }
        }
        int generateRandomNumber()
        {
            Random rand = new Random();
            int RandomNumber = 10000 * rand.Next();
            if (RandomNumber <= 0)
            {
                RandomNumber *= -1;
            }

            return RandomNumber;
        }
        
        void calcReceipt()
        {

            BasePrice_tb.Text = discount_tb.Text = FinalPrice_tb.Text = "0";
            
            //calc actual price
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
               
                BasePrice_tb.Text = ((float.Parse(categorydt.Rows[i]["quantity"].ToString()) * float.Parse(categorydt.Rows[i]["catActualPrice"].ToString())) + float.Parse(BasePrice_tb.Text)).ToString();
                //basePrice_tb.Text = (every_row + tb).ToString();
            }
            //calc discount price
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                discount_tb.Text = (float.Parse(discount_tb.Text) + (float.Parse(categorydt.Rows[i]["quantity"].ToString()) * float.Parse(categorydt.Rows[i]["catDiscount"].ToString()))).ToString();
            }

            //calc final price
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                FinalPrice_tb.Text = (float.Parse(FinalPrice_tb.Text) + (float.Parse(categorydt.Rows[i]["quantity"].ToString()) * float.Parse(categorydt.Rows[i]["catFinalPrice"].ToString()))).ToString();
            }

        }
        void addingCategory(string SerialNumber)
        {
            if (SerialNumber == null) return;
            if (con.State == ConnectionState.Closed)
                con.Open();
            for (int i = 0; i < categorydt.Rows.Count; i++)
            {
                if (SerialNumber == categorydt.Rows[i]["catSerialNumber"].ToString())
                {
                    itemfound = true;
                    
                        
                    categorydt.Rows[i]["quantity"] = int.Parse(categorydt.Rows[i]["quantity"].ToString()) + 1;
                    calcReceipt();
                    break;
                }
                else itemfound = false;
            }

            if (itemfound == false)
            {

                SqlCommand cmd = new SqlCommand("Select catSerialNumber,catName,catActualPrice,catDiscount,catFinalPrice from category where catSerialNumber = @enteredID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@enteredID", SerialNumber);
                SqlDataReader reader = cmd.ExecuteReader();

                categorydt.Load(reader);

                if (categorydt.Rows.Count > 0)
                {   

                    membersDataGrid.ItemsSource = categorydt.DefaultView;

                    membersDataGrid.Columns[0].Header = "السريال";
                    membersDataGrid.Columns[1].Header = "اسم المنتج";
                    membersDataGrid.Columns[2].Header = "السعر الأصلي";
                    membersDataGrid.Columns[3].Header = "الخصومات";
                    membersDataGrid.Columns[4].Header = "السعر النهائي";
                    membersDataGrid.Columns[5].Header = "الكمية";
                    if (!categorydt.Columns.Contains("quantity"))
                        categorydt.Columns.Add("quantity");
                        categorydt.Rows[categorydt.Rows.Count - 1]["quantity"] = 1;
                    membersDataGrid.Columns[5].SetCurrentValue(DataGridCell.DataContextProperty, 1);


                    calcReceipt();
                }


                //merchant_name_lbl.Text = membersDataGrid.Columns.Count.ToString();
                reader.Close();
            }
            con.Close();
        }


        // click Events
        private void SellScreen_btn_Click(object sender, RoutedEventArgs e)
        {
           
        }
         private void categoryAdd_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (scanningAvailability == true)
            {
                if (categoryAdd_tb.Text.Length > 0)
                {
                    if (timer.IsEnabled == false)
                    {

                        waitingSecs = 1;
                        timer.Start();
                    }
                }
                else if (categoryAdd_tb.Text.Length == 0)
                {
                    if (timer.IsEnabled == true)
                    {
                        timer.Stop();
                        waitingSecs = 1;
                    }

                }
            }
            else
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    addingCategory(categoryAdd_tb.Text);
                    categoryAdd_tb.Text = "";
                }

            }

        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }
        private bool isMaximized = true;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
        }
        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void minimize_btn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Minimized;
            }

             



        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            SaveAndPrint_btn.IsEnabled = false;
            generateBillNumber();
            manualbtn.IsEnabled = true;
            manualbtn.Cursor = Cursors.Hand;
            autobtn.IsEnabled = false;
            autobtn.Cursor = Cursors.No;

        }
      
        private void customerBay_tb_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == System.Windows.Input.Key.Enter)
            {

                e.Handled = true;
                rest_tb.Text = (float.Parse(CustomerPay_tb.Text) - float.Parse(FinalPrice_tb.Text)).ToString();
                if (int.Parse(rest_tb.Text) < 0)
                {
                    SaveAndPrint_btn.IsEnabled = false;
                }
                else
                {
                    SaveAndPrint_btn.IsEnabled = true;

                }
               
            }
            else if(e.Key == System.Windows.Input.Key.Tab)
          {


          }
      }
      private void SaveAndPrint_btn_Click(object sender, EventArgs e)
      {
          try
          {
              if (con.State == ConnectionState.Closed)
                  con.Open();
              DataTable db_datatable = preparingSaveProcess();
              SqlBulkCopy objBulk = new SqlBulkCopy(con);
              objBulk.DestinationTableName = "SalesHistory";
              objBulk.ColumnMappings.Add(0, 1);
              objBulk.ColumnMappings.Add(1, 2);
              objBulk.ColumnMappings.Add(2, 3);
              objBulk.ColumnMappings.Add(3, 4);
              objBulk.ColumnMappings.Add(4, 5);
              objBulk.ColumnMappings.Add(5, 6);
              objBulk.ColumnMappings.Add(6, 7);
              objBulk.ColumnMappings.Add(7, 8);
              objBulk.WriteToServer(db_datatable);
              SqlCommand cmd = new SqlCommand("insert into sellingBillInfo(SellingBillnumber,BasePrice,Discount,FinalPrice,CustomerPay,Rest,sellTime,MerchantName,sellDate)" +
                      "values(@billnumber,@BasePrice,@Discount,@FinalPrice,@CustomerPay,@Rest,@time,@MerchantName,@date)", con);
              cmd.Parameters.AddWithValue("@billnumber", billnumber);
              cmd.Parameters.AddWithValue("@BasePrice", BasePrice_tb.Text);
              cmd.Parameters.AddWithValue("@Discount", discount_tb.Text);
              cmd.Parameters.AddWithValue("@FinalPrice", FinalPrice_tb.Text);
              cmd.Parameters.AddWithValue("@CustomerPay", CustomerPay_tb.Text);
              cmd.Parameters.AddWithValue("@Rest", rest_tb.Text);
              cmd.Parameters.AddWithValue("@MerchantName", merchant);
              cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm"));
              cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd/MM/yyyy"));
              cmd.ExecuteNonQuery();
              con.Close();

              for(int i =0; i < categorydt.Rows.Count; i++)
              {
                  quant += double.Parse(categorydt.Rows[i]["quantity"].ToString());
              }
              DataTable tempDt = categorydt.Copy();
             PrintWindow printWindow = new PrintWindow(tempDt, merchant, DateTime.Now.ToString("dddd, dd/MM/yyyy HH:mm:ss"),quant,double.Parse(CustomerPay_tb.Text),double.Parse(discount_tb.Text),double.Parse(FinalPrice_tb.Text),double.Parse(rest_tb.Text),billNumber_tb.Text);
              quant = 0;
              try
              {

                  printWindow.Show();
                  this.Close();
              }
              catch (Exception ex2)
              {
                  MessageBox.Show(ex2.Message);
              }
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
      }


      private void categoryAdd_tb_LostFocus(object sender, RoutedEventArgs e)
      {
          if (!string.IsNullOrEmpty(categoryAdd_tb.Text) && categoryAdd_tb.Text.Length > 0)
          {
              category_text.Visibility = Visibility.Collapsed;

          }
          else
          {

              category_text.Visibility = Visibility.Visible;
          }
      }

      private void categoryAdd_tb_GotFocus(object sender, RoutedEventArgs e)
      {
          category_text.Visibility = Visibility.Collapsed;
          categoryAdd_tb.Focus();
            

        }

        private void categoryAdd_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(categoryAdd_tb.Text) && categoryAdd_tb.Text.Length > 0)
            {
               
                category_text.Visibility = Visibility.Collapsed;
            }
            else if (categoryAdd_tb.IsFocused)
            {
                category_text.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                category_text.Visibility = Visibility.Visible;
            }
      }

      private void category_text_MouseDown(object sender, MouseButtonEventArgs e)
      {
          categoryAdd_tb.Focus();
      }

      private void FinalPrice_tb_TextChanged(object sender, TextChangedEventArgs e)
      {
          rest_tb.Text = (float.Parse(CustomerPay_tb.Text) - float.Parse(FinalPrice_tb.Text)).ToString();
          if (int.Parse(rest_tb.Text) < 0)
          {

              SaveAndPrint_btn.IsEnabled = false;
          }
          else
          {

              SaveAndPrint_btn.IsEnabled = true;
          }
      }

      private void RowDefinition_MouseDown(object sender, MouseButtonEventArgs e)
      {

      }

      private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
          if (e.ClickCount == 2)
          {
              if (isMaximized)
              {
                  this.WindowState = WindowState.Normal;
                  this.Width = 1080;
                  this.Height = 720;
                  isMaximized = false;
              }
              else
              {
                  this.WindowState = WindowState.Maximized;
                  isMaximized = true;
              }
          }
      }

      private void membersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
      {
                int n;
            bool s = int.TryParse(((TextBox)e.EditingElement).Text.ToString(), out n);


            if (((TextBox)e.EditingElement).Text == "" || !s)
            {
                e.Cancel = true;
                MessageBox.Show("    يا اعممممي حط قيمة للكمية بطلوا تخلف بقا"+ s.ToString());
                ((TextBox)e.EditingElement).Text = categorydt.Rows[currentselectedindex]["quantity"].ToString();
                membersDataGrid.ItemsSource = categorydt.DefaultView;

            }
            else
            {
                categorydt.Rows[e.Row.GetIndex()]["quantity"] = ((TextBox)e.EditingElement).Text;
                calcReceipt();
            }

        }
        private void membersDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            this.activeCellAtEdit = membersDataGrid.CurrentCell;
             
              if (MessageBox.Show("متأكد من تعديل كمية هذا المنتج", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                
                e.Cancel = true;
                categoryAdd_tb.Focus();
                FocusNavigationDirection focusDirection = FocusNavigationDirection.Last;

                // MoveFocus takes a TraveralReqest as its argument.
                TraversalRequest request = new TraversalRequest(focusDirection);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    membersDataGrid.MoveFocus(request);
                }
            }
            else
            {
                
            }
             
            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        
        private void deleteRow_btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذا المنتج من قائمة الشراء؟", "تحذير!", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                
                
            }
            else
            {
                categorydt.Rows[membersDataGrid.SelectedIndex].Delete();

                categorydt.AcceptChanges();

                calcReceipt();
            }
        }
        
        private void manualbtnborder_MouseDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void autobtnborder_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
          
        }
       
        private void Window_KeyboardInputProviderAcquireFocus(object sender, KeyboardInputProviderAcquireFocusEventArgs e)
        {
           
        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==System.Windows.Input.Key.F1)
                categoryAdd_tb.Focus();
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        { 
            KeyboardEventArgs keyboardEventArgs = new KeyboardEventArgs(Keyboard.PrimaryDevice, 0);
            keyboardEventArgs.RoutedEvent = Keyboard.PreviewKeyDownEvent;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void SellScreen_btn_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void sellwindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          
        }
        Click c = new Click();
        private void mainborder_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
            {
                categoryAdd_tb.Focus();
            }
        }

        private void mainborder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void mainborder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = Border.PreviewMouseLeftButtonDownEvent;
            mainborder.RaiseEvent(args);
        }

        private void mainborder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //categoryAdd_tb.Focus();
           // MessageBox.Show(e.ChangedButton.ToString());
        }

        private void mainborder_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            }
        bool flag = false;
        private void mainborder_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //categoryAdd_tb.Focus();
            CashierName.Text = "X: " + Mouse.GetPosition(this).X.ToString() + " - y : " + Mouse.GetPosition(this).Y.ToString();
            title.Text = e.GetPosition(this).ToString();
            if (flag == false)
            {
                categoryAdd_tb.Focus();
                flag = true;
            }
            /*if (categoryAdd_tb.IsFocused &&categoryAdd_tb.Text.Length>2)
            {
                addingCategory(categoryAdd_tb.Text);
                categoryAdd_tb.Text = "";

            }
             */

        }

        private void minimize_btn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        
        private void mainborder_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void categoryAdd_tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void manualbtn_Click(object sender, RoutedEventArgs e)
        {
            manualbtn.IsEnabled = false;
            manualbtn.Cursor = Cursors.No;
            autobtn.IsEnabled = true;
            autobtn.Cursor = Cursors.Hand;
            category_text.Focus();
            scanningAvailability = false;

        
        }

        private void autobtn_Click(object sender, RoutedEventArgs e)
        {


            manualbtn.IsEnabled = true;
            manualbtn.Cursor = Cursors.Hand;
            autobtn.IsEnabled = false;
            autobtn.Cursor = Cursors.No;
            category_text.Focus();
            scanningAvailability = true;
        }
    }
}

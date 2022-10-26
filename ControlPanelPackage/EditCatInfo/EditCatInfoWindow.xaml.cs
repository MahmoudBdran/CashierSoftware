using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Shapes;

namespace CashierSoftware.ControlPanelPackage.EditCatInfo
{
    /// <summary>
    /// Interaction logic for EditCatInfoWindow.xaml
    /// </summary>
    public partial class EditCatInfoWindow : Window
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");
        string? selectedId;
        string proc_type;
        public EditCatInfoWindow(string proc_type,String catid)
        {
            InitializeComponent();
            this.selectedId = catid;
            this.proc_type = proc_type;
            loadCompatibleView();
            loadMainCat();
            MainCatSelectTB.IsTextCompletionEnabled = true;
            //MainCatSelectTB.ItemsSource = new String[] { "Ali abdelhameed", "Mahmoud bdran", "Emad abo elhassan", "Hussein el3agramy" };

            /*
            tb.filter
            tb.ItemsSource = new String[] { "Ali abdelhameed" , "Mahmoud bdran" , "Emad abo elhassan" , "Hussein el3agramy"};
            if (tb.IsFocused)
            {
                tb.IsDropDownOpen = true;
                tb.AllowDrop = true;
            }
             */
        }

        public void loadMainCat()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter cmd = new SqlDataAdapter("Select MainCatName from MainCategory",con);
                
                DataTable dt = new DataTable();
                cmd.Fill(dt);

                MainCatSelectTB.FilterMode = AutoCompleteFilterMode.Contains;

                List<string> maincats = new List<string>();
                List<DataRow> list = dt.AsEnumerable().ToList();
                foreach (var row in list)
                {

                    Trace.WriteLine(row["MainCatName"] as string);
                    maincats.Add(row["MainCatName"] as string);
                }
                MainCatSelectTB.ItemsSource = maincats;
                Trace.WriteLine("-----------------------------------");
                Trace.WriteLine(maincats[0] + " : "+ maincats[2]);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void loadCompatibleView() { 
             if(proc_type == "edit")
             {
                fillTextBoxes();
                editpage_tblock.Text = "Edit Page";
                ButtonText.Text = "تحديث";
                IdViewer.Visibility = Visibility.Visible;
             }
             else if(proc_type == "insert")
             {

                editpage_tblock.Text = "Insert Page";
                ButtonText.Text = "حفظ";
                IdViewer.Visibility = Visibility.Hidden;
                ActualpriceViewer.CustomText = "0";
                DiscountViewer.CustomText = "0";
                FinalpriceViewer.CustomText = "0";
                AmountViewer.CustomText = "0";

            }
        
        
        }

        void fillTextBoxes()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
            
                SqlDataAdapter cmd = new SqlDataAdapter("SELECT id,catName,catActualPrice,catDiscount,catFinalPrice,catAmount,catSerialNumber,catMainName FROM category WHERE id = @selectedId", con);
                cmd.SelectCommand.Parameters.AddWithValue("@selectedId",selectedId);
                DataTable dt = new DataTable();
                cmd.Fill(dt);
                //امسح الهبل دا .. الايرور في قراية الداتا بيز .. الداتا تيبل و الداتا ريدر قبل كدا مبيجيبوش داتا
                //اتحلت يا نجم متنساش تعمل fill cmd to datatable => cmd.fill(dt)  ياغبي
                foreach (DataRow dr in dt.Rows)
                {
                    IdViewer.CustomText = dr["id"].ToString();
                    NameViewer.CustomText = dr["catName"].ToString();
                    ActualpriceViewer.CustomText = dr["catActualPrice"].ToString();
                    DiscountViewer.CustomText = dr["catDiscount"].ToString();
                    FinalpriceViewer.CustomText = dr["catFinalPrice"].ToString();
                    AmountViewer.CustomText = dr["catAmount"].ToString();
                    SerialnumberViewer.CustomText = dr["catSerialNumber"].ToString();
                    MainCatSelectTB.Text = dr["catMainName"].ToString();
                }

                
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot Load data from Database : " + e.Message);
            }
        }
        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void ActualpriceViewer_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (ActualpriceViewer.CustomText != null && DiscountViewer.CustomText != null)
            {
                _ = double.TryParse(DiscountViewer.CustomText.ToString(), out double disc);
                _ = double.TryParse(ActualpriceViewer.CustomText.ToString(), out double actu);
                if (ActualpriceViewer.CustomText.Length > 0)
                {
                    if (double.TryParse(ActualpriceViewer.CustomText, out _) || double.TryParse(ActualpriceViewer.CustomText, out _))
                    {
                        double formula1 = actu - disc;

                        FinalpriceViewer.CustomText = formula1.ToString();
                    }
                    else
                    {


                        ActualpriceViewer.CustomText = ActualpriceViewer.CustomText.Substring(0, ActualpriceViewer.CustomText.Length - 1);
                        MessageBox.Show("ما تصحصح ياااااااعممممممممم");
                    }
                }
                else
                {
                    ActualpriceViewer.CustomText = "0";
                }
            }

        }

        private void DiscountViewer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ActualpriceViewer.CustomText != null && DiscountViewer.CustomText != null)
            {
                _ = double.TryParse(DiscountViewer.CustomText.ToString(), out double disc);
                _ = double.TryParse(ActualpriceViewer.CustomText.ToString(), out double actu);
                if (DiscountViewer.CustomText.Length > 0)
                {
                    if (double.TryParse(DiscountViewer.CustomText, out _) || double.TryParse(DiscountViewer.CustomText, out _))
                    {
                        double formula1 = actu - disc;

                        FinalpriceViewer.CustomText = formula1.ToString();
                    }
                    else
                    {


                        DiscountViewer.CustomText = DiscountViewer.CustomText.Substring(0 , DiscountViewer.CustomText.Length -1);
                        MessageBox.Show("ما تصحصح ياااااااعممممممممم");
                    }
                }
                else
                {
                    DiscountViewer.CustomText = "0";
                }
            }
        }

        private void AmountViewer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AmountViewer.CustomText != null)
            {
                if (AmountViewer.CustomText.Length > 0)
                {
                    if (double.TryParse(AmountViewer.CustomText, out _) || double.TryParse(AmountViewer.CustomText, out _))
                    {

                    }
                    else
                    {


                        AmountViewer.CustomText = AmountViewer.CustomText.Substring(0, AmountViewer.CustomText.Length - 1);
                        MessageBox.Show("ما تصحصح ياااااااعممممممممم");
                    }
                }
                else
                {
                    AmountViewer.CustomText = "0";
                }
            }
        }

        private void savecatNewInfo_Click(object sender, RoutedEventArgs e)
        {
            if (proc_type=="edit")
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE category " +
                        "SET catName = @catName , catActualPrice= @catActualPrice , catDiscount =@catDiscount ," +
                        " catFinalPrice =@catFinalPrice , catAmount = @catAmount , catSerialNumber=@catSerialNumber , catMainName=@catMainName " +
                        "WHERE id = @selectedId", con);
                    cmd.Parameters.AddWithValue("@selectedId", IdViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catName", NameViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catActualPrice", ActualpriceViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catDiscount", DiscountViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catFinalPrice", FinalpriceViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catSerialNumber", SerialnumberViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catMainName", MainCatSelectTB.Text);
                    cmd.Parameters.AddWithValue("@catAmount", AmountViewer.CustomText);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("تم تعديل بيانات المنتج بنجاح");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot Load data from Database : " + ex.Message);
                }
                finally
                {
                    this.Close();
                }
            }
            else if(proc_type=="insert"){

                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO category (catName,catActualPrice,catDiscount,catFinalPrice,catSerialNumber,catMainName,catAmount)" +
                        "VALUES (@catName,@catActualPrice,@catDiscount,@catFinalPrice,@catSerialNumber,@catMainName,@catAmount)", con);


                    /* SqlCommand cmd = new SqlCommand("UPDATE category " +
                        "SET catName = @catName , catActualPrice= @catActualPrice , catDiscount =@catDiscount ," +
                        " catFinalPrice =@catFinalPrice , catAmount = @catAmount , catSerialNumber=@catSerialNumber , catMainName=@catMainName " +
                        "WHERE id = @selectedId", con); */
                    //cmd.Parameters.AddWithValue("@selectedId", IdViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catName", NameViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catActualPrice", ActualpriceViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catDiscount", DiscountViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catFinalPrice", FinalpriceViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catSerialNumber", SerialnumberViewer.CustomText);
                    cmd.Parameters.AddWithValue("@catMainName", MainCatSelectTB.Text);
                    cmd.Parameters.AddWithValue("@catAmount", AmountViewer.CustomText);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("تم إضافة المنتج بنجاح");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot save data to Database : " + ex.Message);
                }
                finally
                {
                    this.Close();
                }
            }
             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //fillTextBoxes();
            /*BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();*/
        }


        private void MainCatSelectTB_GotFocus(object sender, RoutedEventArgs e)
        {
            MainCatSelectTB.IsDropDownOpen = true;
        }

        private void MainCatSelectTB_LostFocus(object sender, RoutedEventArgs e)
        {

            MainCatSelectTB.IsDropDownOpen = false;
        }
        /*
private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
{
TextProgressBar.Visibility = Visibility.Visible;
this.IsEnabled = false;
TextProgressBar.Value = e.ProgressPercentage;

}

private void worker_DoWork(object sender, DoWorkEventArgs e)
{
var worker = sender as BackgroundWorker;
worker.ReportProgress(0,String.Format("Processing"));
worker.ReportProgress(1, String.Format("started Processing"));
try
{
if (con.State == ConnectionState.Closed)
 con.Open();
worker.ReportProgress(10,"next step Processing");
Thread.Sleep(1000);
SqlCommand cmd = new SqlCommand("SELECT id,catName,catActualPrice,catDiscount,catFinalPrice,catAmount,catSerialNumber,catMainName FROM category WHERE id = @selectedId", con);
cmd.Parameters.AddWithValue("@selectedId", int.TryParse(selectedId, NumberStyles.Integer, CultureInfo.InvariantCulture, out _));
SqlDataReader dr = cmd.ExecuteReader();
worker.ReportProgress(40, "Still Processing");
Thread.Sleep(1000);
if (dr.Read())
{

 IdViewer.textBox.Text = dr["id"].ToString();
 NameViewer.textBox.Text = dr["catName"].ToString();
 ActualpriceViewer.textBox.Text = dr["catActualPrice"].ToString();
 DiscountViewer.textBox.Text = dr["catDiscount"].ToString();
 FinalpriceViewer.textBox.Text = dr["catFinalPrice"].ToString();
 AmountViewer.textBox.Text = dr["catAmount"].ToString();
 SerialnumberViewer.textBox.Text = dr["catSerialNumber"].ToString();
 MViewerainnameViewer.textBox.Text = dr["catMainName"].ToString();

}
dr.Close();
}
catch (Exception ex)
{
MessageBox.Show("Cannot Load data from Database : " + ex.Message);
}
worker.ReportProgress(100,"Done Processing");

}

private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
{
this.IsEnabled = true;
TextProgressBar.Value = 0;
MessageBox.Show("All Data Retrieved successfully from DB");

}*/
    }
}

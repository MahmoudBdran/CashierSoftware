using CashierSoftware.SellScreenPackage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace CashierSoftware.ControlPanelPackage.AccountsUSerControl
{
    /// <summary>
    /// Interaction logic for AccountsUC.xaml
    /// </summary>
    public partial class AccountsUC : UserControl
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");

        public AccountsUC()
        {
            InitializeComponent();
            loadManagerDGData();
            loadSalesDGData();
        }
        

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (!string.IsNullOrEmpty(email_tb.Text) && !string.IsNullOrEmpty(password_tb.Password) && password_tb.Password == Confirmpassword_tb.Password)
                {
                    //MessageBox.Show("Successfully Signed In");
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    if (CreateAccountmanager_rb.IsChecked == true)
                    {

                        //SqlCommand sqlcmd = new SqlCommand("SELECT ManagerUsername,ManagerPassword from ManagerInfo where ManagerUsername = @mgr_username and ManagerPassword = @mgr_password", con);
                        
                        if (checkinsertManagerAccount(email_tb.Text.Trim()))
                        {
                            SqlCommand sqlcmd = new SqlCommand("insert into ManagerInfo(ManagerUsername,ManagerPassword) values (@mgr_username,@mgr_password)", con);
                            sqlcmd.CommandType = CommandType.Text;
                            sqlcmd.Parameters.AddWithValue("@mgr_username", email_tb.Text.Trim());
                            sqlcmd.Parameters.AddWithValue("@mgr_password", password_tb.Password.Trim());
                            sqlcmd.ExecuteNonQuery();
                            MessageBox.Show("تم حفظ الحساب بنجاح");
                            loadManagerDGData();
                        }
                        else
                        {
                            MessageBox.Show("جرب إسم مختلف", "حساب موجود بالفعل !", MessageBoxButton.OK, MessageBoxImage.Error);
                            email_tb.Text = "";
                            password_tb.Password = "";
                            Confirmpassword_tb.Password = "";
                        }


                    }
                    else if (CreateAccountsales_rb.IsChecked == true)
                    {
                        //SqlCommand sqlcmd = new SqlCommand("SELECT salesUsername,salesPassword from salesAccounts where salesUsername = @salesUsername and salesPassword = @salesPassword", con);
                        
                        if (checkinsertSalesAccount(email_tb.Text.Trim()))
                        {
                            SqlCommand sqlcmd = new SqlCommand("insert into salesAccounts(salesUsername,salesPassword) values (@salesUsername,@salesPassword)", con);
                            sqlcmd.CommandType = CommandType.Text;
                            sqlcmd.Parameters.AddWithValue("@salesUsername", email_tb.Text.Trim());
                            sqlcmd.Parameters.AddWithValue("@salesPassword", password_tb.Password.Trim());
                            sqlcmd.ExecuteNonQuery();
                            MessageBox.Show("تم حفظ الحساب بنجاح");
                            loadSalesDGData();
                        }
                        else
                        {
                            MessageBox.Show("جرب إسم مختلف", "حساب موجود بالفعل !", MessageBoxButton.OK, MessageBoxImage.Error);
                            email_tb.Text = "";
                            password_tb.Password = "";
                            Confirmpassword_tb.Password = "";
                        }



                    }
                }
                else if (email_tb.Text == null)
                {
                    MessageBox.Show("اكتب اسم الحساب", "ERROR");
                }
                else if (password_tb.Password == null)
                {
                    MessageBox.Show("اكتب كلمة السر", "ERROR");
                }
                else if (password_tb.Password != Confirmpassword_tb.Password )
                {
                    MessageBox.Show("كلمتا السر غير متطابقتان !","ERROR");
                }
                else
                {
                    MessageBox.Show("هناك خطأ", "ERROR");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }

        }

        private void loadSalesDGData()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from salesAccounts", con);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            salesaccounts.ItemsSource = dt.DefaultView;

        }
        private void loadManagerDGData()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from ManagerInfo", con);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            manageraccounts.ItemsSource = dt.DefaultView;
        }
        private bool checkinsertSalesAccount(string username)
        {
            SqlCommand cmd = new SqlCommand("SELECT salesUsername from salesAccounts where salesUsername = @salesUsername", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@salesUsername", username);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                dr.Close();
                return true;

            }
            else
            {
                dr.Close();

                return false;
            }

        }
        public bool checkinsertManagerAccount(String username)
        {
            SqlCommand cmd = new SqlCommand("SELECT ManagerUsername,ManagerPassword from ManagerInfo where ManagerUsername = @mgr_username ", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@mgr_username", username);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                dr.Close();
                return true;
                
            }
            else
            {
                dr.Close();
                
                return false;
            }
        }
        private void email_tb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(email_tb.Text) && email_tb.Text.Length > 0)
                textEmail.Visibility = Visibility.Collapsed;
            else if (email_tb.IsFocused)
                textEmail.Visibility = Visibility.Collapsed;
            else
                textEmail.Visibility = Visibility.Visible;
        }
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {

            email_tb.Focus();
        }
        private void email_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(email_tb.Text) && email_tb.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;

            }
            else
            {

                textEmail.Visibility = Visibility.Visible;
            }

        }
        private void email_tb_GotFocus(object sender, RoutedEventArgs e)
        {

            textEmail.Visibility = Visibility.Collapsed;
        }
        private void password_tb_GotFocus(object sender, RoutedEventArgs e)
        {

            textPassword.Visibility = Visibility.Collapsed;
        }
        private void password_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(password_tb.Password) && password_tb.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;

            }
            else
            {

                textPassword.Visibility = Visibility.Visible;
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(password_tb.Password) && password_tb.Password.Length > 0)
                textPassword.Visibility = Visibility.Collapsed;
            else if (password_tb.IsFocused)
                textPassword.Visibility = Visibility.Collapsed;
            else
                textPassword.Visibility = Visibility.Visible;
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            password_tb.Focus();
        }
        private void confirmpassword_tb_GotFocus(object sender, RoutedEventArgs e)
        {

            textConfirmPassword.Visibility = Visibility.Collapsed;
        }
        private void confirmpassword_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Confirmpassword_tb.Password) && Confirmpassword_tb.Password.Length > 0)
            {
                textConfirmPassword.Visibility = Visibility.Collapsed;

            }
            else
            {

                textConfirmPassword.Visibility = Visibility.Visible;
            }
        }
        private void confirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Confirmpassword_tb.Password) && Confirmpassword_tb.Password.Length > 0)
                textConfirmPassword.Visibility = Visibility.Collapsed;
            else if (Confirmpassword_tb.IsFocused)
                textConfirmPassword.Visibility = Visibility.Collapsed;
            else
                textConfirmPassword.Visibility = Visibility.Visible;
        }
        private void textconfirmPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Confirmpassword_tb.Focus();
        }
        private void DG_edit_btn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void DG_remove_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


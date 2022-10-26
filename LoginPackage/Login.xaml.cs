using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;
using CashierSoftware.ControlPanelPackage;
using CashierSoftware.SellScreenPackage;
using CashierSoftware.SellScreenPackage.printPackage;
using IronBarCode;

namespace CashierSoftware.LoginPackage
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\c# projects\cashierApp\cashierAppDB.mdf;Integrated Security=True;Connect Timeout=30");
        public Login()
        {
            InitializeComponent();
            /*  email_tb.Text = "محمود بدران";
              password_tb.Password = "123456";
              manager_rb.IsChecked=true;
             */
            email_tb.Text = "احمد";
            password_tb.Password = "123456";
           sales_rb.IsChecked = true;
        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
          {
              if (e.ChangedButton == MouseButton.Left)
                  this.DragMove();
          }

          private void Image_MouseUp(object sender, MouseButtonEventArgs e)
          {
              Application.Current.Shutdown();
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

          private void Button_Click(object sender, RoutedEventArgs e)
          {
            try
            {
                if(con.State==ConnectionState.Closed)
                    con.Open();
                if (!string.IsNullOrEmpty(email_tb.Text) && !string.IsNullOrEmpty(password_tb.Password))
                {
                    //MessageBox.Show("Successfully Signed In");
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    if (manager_rb.IsChecked==true && email_tb.Text == "mahmoud.bdran20" && password_tb.Password == "mahmoudbdran")//01205057427mahmoudbdran  @gmail.com
                    {
                        try
                        {

                            this.Hide();
                            ControlPanel controlpanel = new ControlPanel();
                            controlpanel.Show();
                            email_tb.Text = "";
                            password_tb.Password = "";
                        }
                        finally
                        {
                            this.Show();
                        }
                    }
                    else if (manager_rb.IsChecked == true)
                    {
                        SqlCommand sqlcmd = new SqlCommand("SELECT ManagerUsername,ManagerPassword from ManagerInfo where ManagerUsername = @mgr_username and ManagerPassword = @mgr_password", con);
                        sqlcmd.CommandType = CommandType.Text;
                        sqlcmd.Parameters.AddWithValue("@mgr_username", email_tb.Text.Trim());
                        sqlcmd.Parameters.AddWithValue("@mgr_password", password_tb.Password.Trim());
                        SqlDataReader dr = sqlcmd.ExecuteReader();
                        if (dr.Read())
                        {
                            dr.Close();
                            try
                            {
                                ControlPanel controlpanel = new ControlPanel();
                                this.Hide();
                                controlpanel.Show();
                                email_tb.Text = "";
                                password_tb.Password = "";
                            }
                            finally
                            {
                                this.Show();
                            }
                        }
                        else
                        {
                            dr.Close();
                            MessageBox.Show("خطأ فى اسم المستخدم أو كلمةالمرور", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                    else if (sales_rb.IsChecked==true)
                    {
                        SqlCommand sqlcmd = new SqlCommand("SELECT salesUsername,salesPassword from salesAccounts where salesUsername = @salesUsername and salesPassword = @salesPassword", con);
                        sqlcmd.CommandType = CommandType.Text;
                        sqlcmd.Parameters.AddWithValue("@salesUsername", email_tb.Text.Trim());
                        sqlcmd.Parameters.AddWithValue("@salesPassword", password_tb.Password.Trim());
                        SqlDataReader dr = sqlcmd.ExecuteReader();
                        if (dr.Read())
                        {
                            dr.Close();
                            try
                            {
                                SellScreen sellingScreen = new SellScreen(email_tb.Text.Trim());
                                this.Close();
                                sellingScreen.Show();
                                // email_tb.Text = "";
                                //password_tb.Password = "";

                            }
                            catch
                            {

                            }
                            

                        }
                        else
                        {
                            dr.Close();
                            MessageBox.Show("خطأ فى اسم المستخدم أو كلمةالمرور", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"ERROR");
            }
              
          }

          private void email_tb_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
          {
              if (!string.IsNullOrEmpty(email_tb.Text) && email_tb.Text.Length > 0)
                  textEmail.Visibility = Visibility.Collapsed;
              else if(email_tb.IsFocused)
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
    }
}


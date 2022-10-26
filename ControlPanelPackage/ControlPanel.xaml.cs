using CashierSoftware.ControlPanelPackage.AccountsUSerControl;
using CashierSoftware.ControlPanelPackage.SalesDashboardUserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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

namespace CashierSoftware.ControlPanelPackage
{
    public partial class ControlPanel : Window
    {
        public ControlPanel()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool isMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                WindowState = WindowState.Normal;
            }
        }

        private void CategoryInfo_btn_Click(object sender, RoutedEventArgs e)
        {
            categoryInfoClass categoryInfo = new categoryInfoClass();
            if (mainPanel.Children.Count > 0) mainPanel.Children.RemoveAt(0);
            mainPanel.Children.Add(categoryInfo as UIElement);
           
        }

        private void SalesDashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            SalesDashboard salesDashboard = new SalesDashboard();
            if (mainPanel.Children.Count > 0) mainPanel.Children.RemoveAt(0);
            mainPanel.Children.Add(salesDashboard as UIElement);
        }

        private void CreateAccountsBtn_Click(object sender, RoutedEventArgs e)
        {
            AccountsUC accounts = new AccountsUC();
            if (mainPanel.Children.Count > 0) mainPanel.Children.RemoveAt(0);
            mainPanel.Children.Add(accounts as UIElement);
        }

        private void logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
  
}

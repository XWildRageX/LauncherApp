using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PassengerTransportationProject.Classes;
using PassengerTransportationProject.Entities;
using PassengerTransportationProject.Model;
using PassengerTransportationProject.Pages;

namespace PassengerTransportationProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string Login = Properties.Settings.Default.LoginUser;
            string Password = Properties.Settings.Default.PasswordUser;
            Passenger PassengerAuthtorization = DB.entities.Passenger.FirstOrDefault(c => c.Login == Login && c.Password == Password);
            if (PassengerAuthtorization != null)
            {
                 CurrentPassenger.passenger = PassengerAuthtorization;
                 GridBar.Visibility = Visibility.Visible;
                 MainFrame.Navigate(new RoutesPage());
            }
            else
            {
                GridBar.Visibility = Visibility.Hidden;
                MainFrame.Navigate(new AuthtorizationPage());
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LoginUser = "";
            Properties.Settings.Default.PasswordUser = "";
            Properties.Settings.Default.Save();
            CurrentPassenger.passenger = null;
            DB.entities.SaveChanges();
            GridBar.Visibility= Visibility.Hidden;
            MainFrame.Navigate(new AuthtorizationPage());
        }

        private void MyTicketsButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new MyTicketsPage());
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            //Directory.Delete("123");
            MainFrame.Navigate(new RoutesPage());
        }

        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWin.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AuthtorizationButton_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenuUser.Visibility = Visibility.Visible;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MyProfileButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new MyProfilePage(CurrentPassenger.passenger));
        }
    }
}

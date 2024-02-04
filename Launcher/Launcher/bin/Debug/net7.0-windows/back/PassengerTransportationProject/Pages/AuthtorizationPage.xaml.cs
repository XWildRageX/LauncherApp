using PassengerTransportationProject.Entities;
using PassengerTransportationProject.Model;
using System;
using System.Collections.Generic;
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

namespace PassengerTransportationProject.Pages
{

    public partial class AuthtorizationPage : Page
    {
        public AuthtorizationPage()
        {
            InitializeComponent();
        }
        private void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.MainWin.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Login = LoginTextBox.Text;
                string Password = PasswordTextBox.Password;
                Passenger PassengerAuthtorization = DB.entities.Passenger.FirstOrDefault(c => c.Login == Login && c.Password == Password);
                if (PassengerAuthtorization != null)
                {
                    if (SaveMeCheck.IsChecked == true)
                    {
                        Properties.Settings.Default.LoginUser = LoginTextBox.Text;
                        Properties.Settings.Default.PasswordUser = PasswordTextBox.Password;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.LoginUser = "";
                        Properties.Settings.Default.PasswordUser = "";
                        Properties.Settings.Default.Save();
                    }
                    MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    CurrentPassenger.passenger = PassengerAuthtorization;
                    mainWindow.GridBar.Visibility = Visibility.Visible;   
                    NavigationService.Navigate(new RoutesPage());
                }
                else if (SpaceCheck.Check(Login) == true || SpaceCheck.Check(Password) == true)
                {
                    MessageBox.Show("Есть пустые поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}

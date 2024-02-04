using PassengerTransportationProject.Classes;
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

namespace PassengerTransportationProject.Pages
{
    public partial class MyProfilePage : Page
    {
        public MyProfilePage(Passenger passenger)
        {
            InitializeComponent();
            this.DataContext = passenger;
            SurnameTextBox.PreviewTextInput += new TextCompositionEventHandler(SNPTextBox_PreviewTextInput);
            NameTextBox.PreviewTextInput += new TextCompositionEventHandler(SNPTextBox_PreviewTextInput);
            PatronymicTextBox.PreviewTextInput += new TextCompositionEventHandler(SNPTextBox_PreviewTextInput);
            passengers = DB.entities.Passenger.ToList();
            if (CurrentPassenger.passenger.GenderID == 1)
            {
                MaleRButton.IsChecked = true;
            }
            else
            {
                FemaleRButton.IsChecked = true;
            }
        }
        List<Passenger> passengers = new List<Passenger>();

        private void SaveButtonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SpaceCheck.Check(NameTextBox.Text) == true || SpaceCheck.Check(SurnameTextBox.Text) == true)
                {
                    MessageBox.Show("Обязательные поля не заполнены.", "Ошибка" , MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if(DateBirthPicker.SelectedDate >= DateTime.Now)
                {
                    MessageBox.Show("Неверно указана дата рождения.","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                }
                else
                {
                    if (MaleRButton.IsChecked == true)
                    {
                        CurrentPassenger.passenger.GenderID = 1;
                    }
                    else
                    {
                        CurrentPassenger.passenger.GenderID = 2;
                    }
                    DB.entities.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SNPTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length != 1) return;
            if (Char.IsLetter(e.Text, 0)) return;
            e.Handled = true;
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

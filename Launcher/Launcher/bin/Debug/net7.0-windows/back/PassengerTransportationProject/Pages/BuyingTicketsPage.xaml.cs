using PassengerTransportationProject.Classes;
using PassengerTransportationProject.Entities;
using PassengerTransportationProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class BuyingTicketsPage : Page
    {
        Route routetrain;
        MyTicket ticket;
        public BuyingTicketsPage(Route route)
        {
            InitializeComponent();
            routetrain = route;
            ticket = new MyTicket();
            PassportNumberTextBox.PreviewTextInput += new TextCompositionEventHandler(TextBox_PreviewTextInput);
            NumberPhoneTextBox.PreviewTextInput += new TextCompositionEventHandler(TextBox_PreviewTextInput);
            NationalityComboBox.ItemsSource = DB.entities.Nationality.ToList();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if ((SpaceCheck.Check(NumberPhoneTextBox.Text) || SpaceCheck.Check(PassportNumberTextBox.Text)) == true || NationalityComboBox.Text == "")
            {
                MessageBox.Show("Не все поля заполнены.","Сообщение",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if(PassportNumberTextBox.Text.Length < 4)
            {
                MessageBox.Show("Не правильно введён номер паспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (NumberPhoneTextBox.Text.Length < 11)
            {
                MessageBox.Show("Не правильно введён номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var rand = new Random(); 
                int placenumb = rand.Next(1, 100);
                if (TypeComboBox.Text == "Купе")
                {
                    ticket.CostTicket = routetrain.CompartmentCost;
                }
                else
                {
                    ticket.CostTicket = routetrain.ReservedSeatCost;
                }
                ticket.TransporterID = 1;
                ticket.PassengerID = CurrentPassenger.passenger.PassengerID;
                ticket.PassportNumber = Convert.ToInt32(PassportNumberTextBox.Text);
                ticket.PhoneNumber = NumberPhoneTextBox.Text;
                ticket.PlaceNumber = placenumb;
                ticket.RouteID = routetrain.RouteID;
                ticket.NationalityID = Convert.ToInt32(NationalityComboBox.SelectedIndex) + 1;
                DB.entities.MyTicket.Add(ticket);
                DB.entities.SaveChanges();
                MessageBox.Show("Билет успешно приобретён.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }          
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length != 1) return;
            if (Char.IsDigit(e.Text, 0)) return;
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

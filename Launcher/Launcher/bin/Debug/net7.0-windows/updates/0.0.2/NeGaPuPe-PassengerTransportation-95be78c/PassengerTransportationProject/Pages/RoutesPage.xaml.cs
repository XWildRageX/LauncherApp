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
    public partial class RoutesPage : Page
    {
        public RoutesPage()
        {
            InitializeComponent();
            FromComboBox.ItemsSource = DB.entities.StationDeparture.ToList();
            ToComboBox.ItemsSource = DB.entities.StationTarget.ToList();
            Ticketslist = DB.entities.Route.ToList();
        }
        List<Route> Ticketslist = new List<Route>();

        private void SearchTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromComboBox.Text == "" && ToComboBox.Text == "")
            {
                MessageBox.Show("Вы не выбрали куда и откуда нужно поехать.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
            else
            {
                if (ToComboBox.Text == "")
                {
                    MessageBox.Show("Выберите куда нужно поехать.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (FromComboBox.Text == "")
                {
                    MessageBox.Show("Выберите откуда нужно поехать.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    TicketsListview.ItemsSource = Ticketslist.Where(c => c.StationDeparture.NameDepartureStation == FromComboBox.Text && c.StationTarget.NameTargetStation == ToComboBox.Text);
                    if (TicketsListview.HasItems == false)
                    {
                        MessageTextblock.Visibility = Visibility.Visible;
                        MessageTextblock.Text = "Таких билетов нет.";
                    }
                    else
                    {
                        MessageTextblock.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            Route CurrentRoute = ((Route)((Button)sender).DataContext);
            NavigationService.Navigate(new BuyingTicketsPage(CurrentRoute));
        }
    }
}

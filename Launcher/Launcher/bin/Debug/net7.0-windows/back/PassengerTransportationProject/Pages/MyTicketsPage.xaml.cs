using MaterialDesignThemes.Wpf;
using PassengerTransportationProject.Entities;
using PassengerTransportationProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Xceed.Words.NET;

namespace PassengerTransportationProject.Pages
{
    public partial class MyTicketsPage : Page
    {
        public MyTicketsPage()
        {
            InitializeComponent();
            myTicketsList = DB.entities.MyTicket.ToList();
            MyTicketsListview.ItemsSource = myTicketsList;
        }
        List<MyTicket> myTicketsList = new List<MyTicket>();

        private void SellMyTicket_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите вернуть этот билет?", "Сообщение", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                MyTicket CurrentTicket = ((MyTicket)((PackIcon)sender).DataContext);
                DB.entities.MyTicket.Remove(CurrentTicket);
                DB.entities.SaveChanges();
                myTicketsList = DB.entities.MyTicket.ToList();
                MyTicketsListview.ItemsSource = myTicketsList;
                MessageBox.Show("Выбранный вами билет возвращён.", "Сообщение",MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReplaceKeywordWithValue(DocX document, string keyword, string value)
        {
            foreach (var paragraph in document.Paragraphs)
            {
                if (paragraph.Text.Contains(keyword))
                {
                    paragraph.ReplaceText(keyword, value);
                }
            }
        }

        private void PrintTicket_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var templateDoc = DocX.Load(@"C:\Users\rich4\Downloads\PassengerTransportationProject\PassengerTransportationProject\Resources\ШаблонБилета.docx"))
            {
                MyTicket CurrentTicket = ((MyTicket)((PackIcon)sender).DataContext);

                var rand = new Random(); string ticketNumber = rand.Next(100, 1000).ToString();
                ReplaceKeywordWithValue(templateDoc, "[numberticket]", ticketNumber);

                string stationDeparture = CurrentTicket.Route.StationDeparture.NameDepartureStation;
                ReplaceKeywordWithValue(templateDoc, "[stationdeparture]", stationDeparture);
                string dateDeparture = CurrentTicket.Route.DateDepartureStation.ToString();
                ReplaceKeywordWithValue(templateDoc, "[datedeparture]", dateDeparture);
                string stationTarget = CurrentTicket.Route.StationTarget.NameTargetStation;
                ReplaceKeywordWithValue(templateDoc, "[stationtarget]", stationTarget);
                string dateTarget = CurrentTicket.Route.DateTargetStation.ToString();
                ReplaceKeywordWithValue(templateDoc, "[datetarget]", dateTarget);
                string snppassenger = CurrentTicket.Passenger.Surname + " " + CurrentTicket.Passenger.Name + " " + CurrentTicket.Passenger.Patronymic;
                ReplaceKeywordWithValue(templateDoc, "[snppassenger]", snppassenger);
                string nationality = CurrentTicket.Nationality.Nationality1;
                ReplaceKeywordWithValue(templateDoc, "[nationality]", nationality);
                string npassport = CurrentTicket.PassportNumber.ToString();
                ReplaceKeywordWithValue(templateDoc, "[numberpassport]", npassport);
                string snptransporter = CurrentTicket.Transporter.Name;
                ReplaceKeywordWithValue(templateDoc, "[snptransporter]", snptransporter);
                string nplace = CurrentTicket.PlaceNumber.ToString();
                ReplaceKeywordWithValue(templateDoc, "[numberplace]", nplace);
                string cost = CurrentTicket.CostTicket.ToString() + "₽";
                ReplaceKeywordWithValue(templateDoc, "[costticket]", cost);


                templateDoc.SaveAs($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Билет на поезд");

                MessageBox.Show("Документ успешно создан. Вы можете найти его на рабочем столе.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myTicketsList = DB.entities.MyTicket.ToList();
            MyTicketsListview.ItemsSource = myTicketsList;
        }
    }
}

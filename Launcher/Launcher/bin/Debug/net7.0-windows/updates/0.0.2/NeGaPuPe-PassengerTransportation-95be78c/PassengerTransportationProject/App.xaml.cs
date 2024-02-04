using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Text;
using System.IO.Packaging;
using PassengerTransportationProject.Entities;
using PassengerTransportationProject.Model;

namespace PassengerTransportationProject
{
    public partial class App : Application
    {
        Model.Task task;
        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            task = new Model.Task();
        }
        static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            List<string> result = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);
            switch (ClassItemField)
            {
                case "Capacity":
                    int Capacity = 0;
                    foreach (ManagementObject m in searcher.Get()) Capacity += Convert.ToInt32(Math.Round(Convert.ToDouble(m[ClassItemField]) / 1024 / 1024));
                    result.Add(Capacity.ToString() + " Мб");
                    break;
                default:
                    foreach (ManagementObject obj in searcher.Get()) result.Add(obj[ClassItemField].ToString().Trim());
                    break;
            }
            return result;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception error = e.Exception;
            string ErrorDescription = error.ToString();
            string ErrorMessage = error.Message;
            var result = MessageBox.Show("Возникла критическая ошибка. Отправить отчёт об ошибке на почту разработчика ?","Критическая ошибка",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string Subject = "";
                string Body = "";
                string Date = "";
                string Priority = "";

                string m1 = "goshanmarket@mail.ru";
                string m2 = "rich416@mail.ru";
                string m2SubPassword = "V5ymtdEhf3GbXkZpZHuw";
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                SmtpClient mySmtpClient = new SmtpClient("smtp.mail.ru");
                mySmtpClient.Port = 587;
                mySmtpClient.UseDefaultCredentials = true;
                mySmtpClient.EnableSsl = true;
                System.Net.NetworkCredential basicAuthenticationInfo = new
                System.Net.NetworkCredential(m2, m2SubPassword);
                mySmtpClient.Credentials = basicAuthenticationInfo;

                string Numbers = "1234567890";
                string CodeTask = "";
                Random random = new Random();
                for (int i = 0; i <= 5; i++)
                {
                    CodeTask += Numbers[random.Next(0, Numbers.Length)];
                }

                MailAddress from = new MailAddress(m2);
                MailAddress to;
                to = new MailAddress(m1);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = $@"<br>
<br>Здравствуйте, у меня возникла критическая ошибка в приложении и я хотел бы получить ответ с решением этой проблемы!
<br>
<br><b>Текст ошибки:</b> {ErrorMessage} 
<br><b>Подробное описание:</b> {ErrorDescription}
<br><b>Системные характеристики:</b>
<br>Объём ОЗУ: {GetHardwareInfo("Win32_PhysicalMemory", "Capacity").First()}
<br>Процессор: {GetHardwareInfo("Win32_Processor", "Name").First()}
<br>Производитель процессора: {GetHardwareInfo("Win32_Processor", "Manufacturer").First()}
<br>Видеокарта: {GetHardwareInfo("Win32_VideoController", "Name").First()}
<br>Драйвер видеокарты: {GetHardwareInfo("Win32_VideoController", "DriverVersion").First()}
<br>Диск: {GetHardwareInfo("Win32_DiskDrive", "Caption").First()}
<br>Объём диска(в байтах) {GetHardwareInfo("Win32_DiskDrive", "Size").First()}";

                MailAddress replyTo = new MailAddress(m2);
                mailMessage.ReplyToList.Add(replyTo);
                mailMessage.Subject = "Отчёт об ошибке №" + CodeTask;
                mailMessage.Priority = MailPriority.High;
                mailMessage.SubjectEncoding = Encoding.GetEncoding(1251);
                mailMessage.BodyEncoding = Encoding.UTF8;
                mySmtpClient.Send(mailMessage);

                Subject = mailMessage.Subject;
                Body = mailMessage.Body.Replace("<br>","").Replace("<b>","").Replace("</b>","");
                Date = mailMessage.Headers["Date"];


                string[] randomPriority = new string[3];
                randomPriority[0] = "Низкий";
                randomPriority[1] = "Средний";
                randomPriority[2] = "Высокий";

                Random r = new Random();
                Priority = randomPriority[r.Next(0, randomPriority.Length - 1)];

                task.Subject = Subject;
                task.Priority = Priority;
                task.Text = Body;
                task.DateTask = Convert.ToDateTime(Date);
                DB.entities.Task.Add(task);
                DB.entities.SaveChanges();

                MessageBox.Show("Сообщение об ошибке было отправлено в службу поддержки, ожидайте ответа.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            e.Handled = true;
        }
    }
}

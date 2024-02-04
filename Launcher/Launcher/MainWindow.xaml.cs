using System;
using System.IO;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using Launcher.Enums;
using System.Net.Http;
using System.Net.Http.Json;
using Launcher.Classes;
using System.Linq;
using System.Security.Policy;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LauncherStatus status;
        private LauncherStatus Status
        {
            get => status;
            set
            {
                status = value;
                switch (status)
                {
                    case LauncherStatus.ready:
                        CheckButton.Content = "Начать";
                        break;
                    case LauncherStatus.failed:
                        CheckButton.Content = "Перезагрузить";
                        break;
                    case LauncherStatus.downloading:
                        CheckButton.Content = "Загрузка";
                        break;
                    case LauncherStatus.downloadingUpdate:
                        CheckButton.Content = "Загрузка обновления";
                        break;
                }
            }
        }
        private string rootPath;
        private string updatePath;
        private string versionFile;
        private List<string> tag = new List<string>();
        private List<string> mas = new List<string>();
        private List<string> information = new List<string>();
        private string zip;
        private string exe;



        private async void CheckUpdates()
        {
            if(File.Exists(versionFile))
            {
                Classes.Version localVersion = new Classes.Version(Properties.Settings.Default.NewVersion);
                VersionText.Text = Properties.Settings.Default.NewVersion;
                try
                {
                    var Client = new HttpClient();
                    Client.DefaultRequestHeaders.Add("User-Agent", "Something");
                    var version = await Client.GetFromJsonAsync<Tes>("https://api.github.com/repos/NeGaPuPe/PassengerTransportation/releases/latest");
                    Classes.Version onlineVersion = new Classes.Version(version.Tag_name);
                    if (onlineVersion.IsDifferentThan(localVersion))
                    {
                        if (MessageBox.Show("Доступно обновление!", "Оповещение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) { InstallFiles(true, onlineVersion, version.Zipball_url); }
                        else
                        {
                            Status = LauncherStatus.ready;
                            var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));
                            while(await timer.WaitForNextTickAsync()) 
                            {
                                this.Dispatcher.Invoke(()=>
                                {
                                    CheckUpdates();
                                });
                            }
                        }
                    }
                    else { Status = LauncherStatus.ready; }
                }
                catch(Exception ex)
                {
                    Status =LauncherStatus.failed;
                    MessageBox.Show($"Ошибка обновления: {ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                InstallFiles(false, Classes.Version.zero,"");
            }
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            
        }
        
        private async void InstallFiles(bool update, Classes.Version onlineVersion, string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (update)
                {
                    Status = LauncherStatus.downloadingUpdate;
                }
                else
                {
                    Status = LauncherStatus.downloading;
                    var Client = new HttpClient();
                    Client.DefaultRequestHeaders.Add("User-Agent", "Something");
                    var version = await Client.GetFromJsonAsync<Tes>("https://api.github.com/repos/NeGaPuPe/PassengerTransportation/releases/latest");
                    onlineVersion = new Classes.Version(version.Tag_name);
                    url = version.Zipball_url;
                }
                webClient.Headers.Add("User-Agent", "Something");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);
                var Urls = new Uri(url);
                webClient.DownloadFileAsync(Urls, zip, onlineVersion);
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Ошибка установки файлов: {ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string ver = (string)VerisonCombobox.SelectedValue;
                string onlineVersion = ((Classes.Version)e.UserState).ToString();
                ZipFile.ExtractToDirectory(zip,Path.Combine(updatePath,ver) , true);
                File.Delete(zip);
                File.WriteAllText(versionFile, onlineVersion);
                if (Status == LauncherStatus.downloadingUpdate) { VersionText.Text = onlineVersion; }
                else { VersionText.Text = Properties.Settings.Default.NewVersion; }
                Status = LauncherStatus.ready;
                var backinfo = new FileInfo(Path.Combine(rootPath, "back.zip"));
                if (backinfo.Exists)
                {
                    backinfo.Delete();
                }
                var dir = new DirectoryInfo(Path.Combine(updatePath, ver)).GetDirectories().First(i => i.FullName.Split('/').Last().Contains("NeGaPuPe-PassengerTransportation"));
                ZipFile.CreateFromDirectory(Path.Combine(rootPath, "back"), Path.Combine(rootPath, "back.zip"));
                Directory.Delete(Path.Combine(rootPath, "back"),true);
                DirectoryCopy(dir.FullName, Path.Combine(rootPath, "back"), true);
                Properties.Settings.Default.OldVersion = Properties.Settings.Default.NewVersion;
                Properties.Settings.Default.NewVersion = ver;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Status = LauncherStatus.failed;
                MessageBox.Show($"Ошибка установки: {ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Process Cmd(string line)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden,
            });
        }

        private void PCInfo_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show($"{InfoComputer.GetHardwareInfo("Win32_Processor", "Name")[0]}\n{InfoComputer.GetHardwareInfo("Win32_VideoController", "Name")[0]}\n{InfoComputer.GetHardwareInfo("Win32_DiskDrive", "Caption")[0]}", "Конфигурация ПК", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public async void ComboBoxAdd()
        {
            InitializeComponent();
            var Clientos = new HttpClient();
            Clientos.DefaultRequestHeaders.Add("User-Agent", "Something");
            var version = await Clientos.GetFromJsonAsync<Tes[]>("https://api.github.com/repos/NeGaPuPe/PassengerTransportation/releases");
            foreach (var item in version)
            {
                VerisonCombobox.Items.Add(item.Tag_name);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            rootPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(rootPath, "updates")))
                Directory.CreateDirectory(Path.Combine(rootPath, "updates"));
            var dit = new DirectoryInfo(rootPath).GetDirectories().First(i => i.FullName.Split('/').Last().Contains("updates"));
            updatePath = Path.Combine(dit.FullName);
            versionFile = Path.Combine(rootPath, "Version.txt");
            zip = Path.Combine(rootPath, "PassengerTransportation.zip");
            exe = Path.Combine(rootPath, "Debug", "Practica.exe");
            CheckUpdates();
            ComboBoxAdd();
            VerisonCombobox.SelectedIndex = Properties.Settings.Default.ComboBoxIndex;
            if (DownloadProgress.Value == 100 || Status == LauncherStatus.ready)
            {
                DownloadProgress.Visibility = Visibility.Collapsed;
            }
        }

        private async void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            string ver = (string)VerisonCombobox.SelectedValue;
            string subver = VersionText.Text;
           if (ver == subver)
            {
                var exet = Path.Combine(rootPath,"back", "ShopApp\\bin\\Debug\\ShopApp.exe");
                try
                {
                    Process.Start(exet);                 
                    //Cmd($@"cd {exe} && ShopApp.exe");
                }
                catch { MessageBox.Show("Файл не может быть открыт или повреждён","Ошибка запуска",MessageBoxButton.OK,MessageBoxImage.Error); }
            }
            else if(ver != subver)
            {
                    var dir = new DirectoryInfo(Path.Combine(updatePath, ver));
                   if(dir.Exists)
                {
                    string deletefile = Path.Combine(dir.FullName);
                    Directory.Delete(deletefile, true);
                }
                    AnyVersions();
                    Status = LauncherStatus.ready;
            }
            else if (Status == LauncherStatus.failed)
            {
                CheckUpdates();
            }
            if (DownloadProgress.Value == 100 || Status == LauncherStatus.ready)
            {
                DownloadProgress.Visibility = Visibility.Collapsed;
            }
        }

        private async void AnyVersions()
        {
                try
                {
                    WebClient webClient = new WebClient();
                    var Client = new HttpClient();
                    Client.DefaultRequestHeaders.Add("User-Agent", "Something");
                    var versions = await Client.GetFromJsonAsync<Tes[]>("https://api.github.com/repos/NeGaPuPe/PassengerTransportation/releases");                  
                    foreach (var item in versions)
                    {
                        tag.Add(item.Tag_name);
                    }
                    Classes.Version onlineVersion = new Classes.Version(tag[VerisonCombobox.SelectedIndex]);
                    Status = LauncherStatus.downloading;
                foreach (var item in versions)
                    {
                        mas.Add(item.Zipball_url);  
                    }
                    string url = mas[VerisonCombobox.SelectedIndex];
                    webClient.Headers.Add("User-Agent", "Something");
                DownloadProgress.Maximum = 100;
                if (Status == LauncherStatus.downloading || Status == LauncherStatus.downloadingUpdate)
                {
                    DownloadProgress.Visibility = Visibility.Visible;
                }
                else if (Status == LauncherStatus.ready || DownloadProgress.Value == 100)
                {
                    DownloadProgress.Visibility = Visibility.Collapsed;                    
                }
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                var Urls = new Uri(url);
                webClient.DownloadFileAsync(Urls, zip, onlineVersion);
                DownloadProgress.Visibility = Visibility.Collapsed;
            }
                catch (Exception ex)
                {
                    Status = LauncherStatus.failed;
                    MessageBox.Show($"Ошибка установки файлов: {ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        public void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)

        {
            DownloadProgress.Value = e.ProgressPercentage;
        }

        private async void Info_Click(object sender, RoutedEventArgs e)
        {
                try
                {
                    WebClient webClient = new WebClient();
                    var Client = new HttpClient();
                    Client.DefaultRequestHeaders.Add("User-Agent", "Something");
                    var versions = await Client.GetFromJsonAsync<Tes[]>("https://api.github.com/repos/NeGaPuPe/PassengerTransportation/releases");
                    foreach (var item in versions)
                    {
                        information.Add(item.Body);
                    }
                    MessageBox.Show($"{information[VerisonCombobox.SelectedIndex]}", "Информация об обновлении", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отображения: {ex}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VerisonCombobox.SelectedIndex = Properties.Settings.Default.ComboBoxIndex;
        }

        private void VerisonCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.ComboBoxIndex = VerisonCombobox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void recrate_Click(object sender, RoutedEventArgs e)
        {
            var info = new FileInfo(Path.Combine(rootPath, "back.zip"));
            if(info.Exists) {
                ZipFile.ExtractToDirectory(info.FullName, Path.Combine(rootPath, "back"), true);
                MessageBox.Show("Выполнен возврат на предидущую версию");
                info.Delete();
                Properties.Settings.Default.NewVersion = Properties.Settings.Default.OldVersion;
                Properties.Settings.Default.OldVersion = string.Empty;
                Properties.Settings.Default.Save();
                VersionText.Text = Properties.Settings.Default.NewVersion;
            }
        }

        private static void DirectoryCopy(
        string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void trychek_Click(object sender, RoutedEventArgs e)
        {
            string link = @"https://drive.google.com/uc?authuser=0&id=12MG44dJRmy3F9XN5U758u5LP1axD_huB&export=download"; //ссылка на файл
            WebClient webClient = new WebClient();
            DownloadProgress.Maximum = 100;
            DownloadProgress.Visibility = Visibility.Visible;
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            webClient.Headers.Add("User-Agent", "Something");
            webClient.DownloadFileAsync(new Uri(link), "Лене.mp3");
        }
    }
}

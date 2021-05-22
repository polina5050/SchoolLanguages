using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace SchoolLanguages.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        
        List<Service> ServisList1 = Classes.BD.EM.Service.ToList();
        List<Service> ServisListRefresh = Classes.BD.EM.Service.ToList();
        List<Client> ClientList = Classes.BD.EM.Client.ToList();
        List<Service> ServisList = new List<Service>();
        public Admin()
        {
            InitializeComponent();
            ServisList = ServisList1;
            DGServises.ItemsSource = ServisList;
            CBClients.ItemsSource = Classes.BD.EM.Client.ToList();
            CBClients.SelectedValuePath = "id";
            CBClients.DisplayMemberPath = "People";
            
        }

        int i = -1;
        private void MediaElement_Initialized(object sender, EventArgs e)
        {
            if (i < ServisList.Count)
            {
                i++;
                MediaElement ME = (MediaElement)sender;
                Service S = ServisList[i];
                Uri U = new Uri(S.MainImagePath, UriKind.RelativeOrAbsolute);
                ME.Source = U;
            }

        }

        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            if (i < ServisList.Count)
            {
                TextBlock TB = (TextBlock)sender;
                Service S = ServisList[i];
                TB.Text = S.Title;

            }

        }
        private void StackPanel_Initialized(object sender, EventArgs e)
        {
            if (i < ServisList.Count)
            {
                StackPanel SP = (StackPanel)sender;
                Service S = ServisList[i];
                if (S.Discount != 0)
                {
                    SP.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));
                }
            }
        }

        private void TextBlock_Initialized_1(object sender, EventArgs e)//для цены
        {
            if (i < ServisList.Count)
            {
                TextBlock TС = (TextBlock)sender;
                Service C = ServisList[i];
                int cost = Convert.ToInt32(C.Cost);
                TС.Text = cost + " ";
                if (C.Discount != 0)
                {
                    TС.TextDecorations = TextDecorations.Strikethrough;

                }
            }
        }

        private void TextBlock_Initialized_2(object sender, EventArgs e)//цена со скидкой
        {
            if (i < ServisList.Count)
            {
                TextBlock PriceDis = (TextBlock)sender;
                Service CD = ServisList[i];
                int disc = Convert.ToInt32(CD.Discount * 100);
                int price = Convert.ToInt32(CD.Cost * 100 - (CD.Cost * disc)) / 100;
                if (CD.Discount != 0)
                {
                    PriceDis.Text = Convert.ToString(price);

                }
            }
        }

        private void TextBlock_Initialized_3(object sender, EventArgs e)//скидка
        {
            if (i < ServisList.Count)
            {
                TextBlock Dis = (TextBlock)sender;
                Service D = ServisList[i];
                if (D.Discount != 0)
                {
                    Dis.Text = "*скидка " + D.Discount * 100 + "%";

                }
                else
                {
                    Dis.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Button_Initialized(object sender, EventArgs e)//редактирование
        {
            Button BtnRed = (Button)sender;
            if (BtnRed != null)
            {
                BtnRed.Uid = Convert.ToString(i);
            }

        }
        int ind;
        private void Button_Click(object sender, RoutedEventArgs e)//редактирование
        {
            Button BtnRed = (Button)sender;
            ind = Convert.ToInt32(BtnRed.Uid);
            Service RD = ServisList[ind];
            RedID.Text = Convert.ToString((ind + 1));
            Service SZn = ServisList[ind];
            RedNaz.Text = SZn.Title;
            RedPrice.Text = Convert.ToString(SZn.Cost);
            RedTime.Text = Convert.ToString(SZn.DurationInSeconds/60);
            RedOpis.Text = Convert.ToString(SZn.Description);
            RedSale.Text = Convert.ToString(SZn.Discount);
            RedPath.Text = SZn.MainImagePath;
            DGServises.Visibility = Visibility.Collapsed;
            Redact.Visibility = Visibility.Visible;
            BtnYsl.Visibility = Visibility.Collapsed;
            SFS.Visibility = Visibility.Collapsed;
        }

        private void SaveRed_Click(object sender, RoutedEventArgs e)//сохранение изменений
        {
           Service R = ServisList[ind];
            R.Title = RedNaz.Text;
            R.Cost = Convert.ToDecimal(RedPrice.Text);
            R.DurationInSeconds = Convert.ToInt32(RedTime.Text)*60;
            R.Description = RedOpis.Text;
            R.Discount = Convert.ToDouble(RedSale.Text)/100;
            R.MainImagePath = RedPath.Text;
            Classes.BD.EM.Service.Add(R);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Изменено");
            Global.MF.Navigate(new Pages.Admin());
        }

        private void Button_Initialized_1(object sender, EventArgs e)//удаление
        {
            Button BtnDel = (Button)sender;
            if (BtnDel != null)
            {
                BtnDel.Uid = Convert.ToString(i);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//удаление
        {
            Button BtnDel = (Button)sender;
            int index = Convert.ToInt32(BtnDel.Uid);
            Service S = ServisList[index];
            Classes.BD.EM.Service.Remove(S);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Удалено");
            Global.MF.Navigate(new Pages.Admin());

        }
        int IdServ;
        private void Button_Initialized_2(object sender, EventArgs e)//новый заказ
        {
            Button BtnNew = (Button)sender;
            if (BtnNew != null)
            {
                BtnNew.Uid = Convert.ToString(i);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//новый заказ
        {
            Button BtnNew = (Button)sender;
            int index = Convert.ToInt32(BtnNew.Uid);
            Service S = ServisList[index];
            IdServ = S.ID;
            Client C = ClientList[index];
            BtnYsl.Visibility = Visibility.Collapsed;
            DGServises.Visibility = Visibility.Collapsed;
            NewZap.Visibility = Visibility.Visible;
            SFS.Visibility = Visibility.Collapsed;
            NazZap.Text = S.Title;
            TZap.Text = Convert.ToString(S.DurationInSeconds/60);
        }
        private void IBtn_Click(object sender, RoutedEventArgs e)//кнопка для поиска изображения
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string PathI = OFD.FileName;
            int ZnachPath = PathI.IndexOf("У");
            string Stroka = PathI.Substring(ZnachPath);
            IPath.Text = Stroka;
        }
        
        private void SaveNew_Click(object sender, RoutedEventArgs e)
        {
            Service found = ServisList.Find(item => item.Title == Naz.Text);

            try{
                if (found == null && Convert.ToInt32(Time.Text) <= 240 && Convert.ToInt32(Time.Text) > 0)
                {
                    Service ObjectServ = new Service()
                    {
                        Title = Naz.Text,
                        Cost = Convert.ToInt32(Price.Text),
                        DurationInSeconds = Convert.ToInt32(Time.Text) * 60,
                        Description = Opis.Text,
                        Discount = Convert.ToDouble(Sale.Text) / 100,
                        MainImagePath = IPath.Text
                    };
                    Classes.BD.EM.Service.Add(ObjectServ);
                    Classes.BD.EM.SaveChanges();
                    MessageBox.Show("Услуга добавлена");
                    Global.MF.Navigate(new Pages.Admin());
                }
                else
                {
                    MessageBox.Show("Неверные данные!");
                }
            }
            catch
            {
                MessageBox.Show("Добавьте все поля!");
            }
        }
        
        private void TextBlock_Initialized_4(object sender, EventArgs e)//время
        {
            if (i < ServisList.Count)
            {
                TextBlock time = (TextBlock)sender;
                Service T = ServisList[i];
                time.Text = "за " + Convert.ToInt32(T.DurationInSeconds / 60) + " минут";

            }
        }
        
        private void BtnYsl_Click(object sender, RoutedEventArgs e)//новая запись
        {
            Button BtnEsl = (Button)sender;
            BtnEsl.Visibility = Visibility.Collapsed;
            DGServises.Visibility = Visibility.Collapsed;
            NewYsl.Visibility = Visibility.Visible;
            SFS.Visibility = Visibility.Collapsed;
            WriteZap.IsEnabled = false;
        }
        private void Home2_Click(object sender, RoutedEventArgs e)
        {
            BtnYsl.Visibility = Visibility.Visible;
            DGServises.Visibility = Visibility.Visible;
            NewYsl.Visibility = Visibility.Collapsed;
            SFS.Visibility = Visibility.Visible;
        }
        private void Home_Click(object sender, RoutedEventArgs e)//кнопка возврата
        {
            DGServises.Visibility = Visibility.Visible;
            Redact.Visibility = Visibility.Collapsed;
            BtnYsl.Visibility = Visibility.Visible;
            SFS.Visibility = Visibility.Visible;
            Global.MF.Navigate(new Pages.Admin());
        }
        DateTime DT;
        
        private void StartTime_TextChanged(object sender, TextChangedEventArgs e)//работа с датой и временем
        {
            Regex r1 = new Regex("[0-1][0-9]:[0-5][0-9]");
            Regex r2 = new Regex("2[0-3]:[0-5][0-9]");
            string s = "";
            if (r1.IsMatch(StartTime.Text) || r2.IsMatch(StartTime.Text) && StartTime.Text.Length == 5)
            {
                TimeSpan TS = TimeSpan.Parse(StartTime.Text);
                DT = Convert.ToDateTime(DateZ.SelectedDate);
                DT = DT.Add(TS);
                if(DT> DateTime.Now)
                {
                    MessageBox.Show(DT + "");
                    WriteZap.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Слишком поздно");
                    WriteZap.IsEnabled = false;
                }
            }
            else
            {
                if (StartTime.Text.Length >= 5)
                {
                    MessageBox.Show("Неверный формат времени!");
                    WriteZap.IsEnabled = false;
                }
            }
        }

        private void WriteZap_Click(object sender, RoutedEventArgs e)
        {
            int IdClient = CBClients.SelectedIndex + 1;
            ClientService ObjectSC = new ClientService()
            {

                ClientID = IdClient,
                ServiceID = IdServ,
                StartTime = DT,

            };
            Classes.BD.EM.ClientService.Add(ObjectSC);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Запись добавлена");
            Global.MF.Navigate(new Pages.Admin());
        }

        private void Home3_Click(object sender, RoutedEventArgs e)
        {
            NewZap.Visibility = Visibility.Collapsed;
            DGServises.Visibility = Visibility.Visible;
            BtnYsl.Visibility = Visibility.Visible;
            SFS.Visibility = Visibility.Visible;
        }

        private void SortUp_Click(object sender, RoutedEventArgs e)
        {
            i = -1;
            ServisList.Sort((x, y) => x.Cost.CompareTo(y.Cost));
            DGServises.Items.Refresh();
            ColZap.Visibility = Visibility.Visible;
            All.Text = Convert.ToString(ServisList1.Count);
            Vivod.Text = Convert.ToString(ServiseListFilter.Count);
        }

        private void SortDown_Click(object sender, RoutedEventArgs e)
        {
            i = -1;
            ServisList.Sort((x, y) => x.Cost.CompareTo(y.Cost));
            ServisList.Reverse();
            DGServises.Items.Refresh();
            ColZap.Visibility = Visibility.Visible;
            All.Text = Convert.ToString(ServisList1.Count);
            Vivod.Text = Convert.ToString(ServiseListFilter.Count);

        }
        List<Service> ServiseListFilter = new List<Service>();
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)//фильтрация по скидке
        {
            i = -1;

            switch (Filter.SelectedIndex)
            {
                case 0:
                    if (Poisk.Text != " ")
                    {
                        ServisList = ServisListRefresh;
                        ServiseListFilter = ServisList.Where(x => x.Discount < 0.05 && x.Title.Contains(Poisk.Text)).ToList();//фильтрация после поиска
                        ServisList = ServiseListFilter;
                        DGServises.ItemsSource = ServisList;
                    }
                    else
                    {

                        ServisList = ServisListRefresh;
                        ServiseListFilter = ServisList.Where(x => x.Discount < 0.05).ToList();
                        ServisList = ServiseListFilter;
                        DGServises.ItemsSource = ServisList;
                    }
                        break;
                    
                case 1:
                    ServisList = ServisListRefresh;
                    ServiseListFilter = ServisList.Where(x => x.Discount < 0.15 && x.Discount >= 0.05).ToList();
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                    break;
                case 2:
                    ServisList = ServisListRefresh;
                    ServiseListFilter = ServisList.Where(x => x.Discount < 0.30 && x.Discount >= 0.15).ToList();
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                    break;
                case 3:
                    ServisList = ServisListRefresh;
                    ServiseListFilter = ServisList.Where(x => x.Discount < 0.70 && x.Discount >= 0.30).ToList();
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                    break;
                case 4:
                    ServisList = ServisListRefresh;
                    ServiseListFilter = ServisList.Where(x => x.Discount < 0.100 && x.Discount >= 0.70).ToList();
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                    break;
                case 5:
                    ServisList = ServisListRefresh;
                    ServiseListFilter = ServisList.Where(x => x.Discount <= 100 && x.Discount >= 0).ToList();
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                    break;
            }
            ColZap.Visibility = Visibility.Visible;
            All.Text = Convert.ToString(ServisList1.Count);
            Vivod.Text = Convert.ToString(ServiseListFilter.Count);
        }

        private void Poisk_TextChanged(object sender, TextChangedEventArgs e)//поиск по названию
        {
            i = -1;
            if (Poisk.Text != "")
            {
                List<Service> ServiseListPoisk = new List<Service>();
                ServiseListPoisk = ServisList.Where(x => x.Title.Contains(Poisk.Text)).ToList();
                ServisList = ServiseListPoisk;
                DGServises.ItemsSource = ServisList;
            }
            else
            {
                if (ServiseListFilter.Count == 0)
                {
                    ServisList = ServisList1;
                    DGServises.ItemsSource = ServisList;
                }
                else
                {
                    ServisList = ServiseListFilter;
                    DGServises.ItemsSource = ServisList;
                }
            }
            ColZap.Visibility = Visibility.Visible;
            All.Text = Convert.ToString(ServisList1.Count);
            Vivod.Text = Convert.ToString(ServiseListFilter.Count);
        }
    }
}



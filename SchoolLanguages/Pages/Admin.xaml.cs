using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace SchoolLanguages.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        List<Service> ServisList = Classes.BD.EM.Service.ToList();
        public Admin()
        {
            InitializeComponent();
            DGServises.ItemsSource = ServisList;
            
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
                    Dis.Text = "*скидка" + D.Discount * 100 + "%";

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
            RedSale.Text = Convert.ToString(SZn.Discount*100);
            RedPath.Text = SZn.MainImagePath;
            DGServises.Visibility = Visibility.Collapsed;
            Redact.Visibility = Visibility.Visible;
            BtnYsl.Visibility = Visibility.Collapsed;
        }

        private void SaveRed_Click(object sender, RoutedEventArgs e)//сохранение изменений
        {
           Service R = ServisList[ind];
            //List<Service> Ser = Classes.BD.EM.Service.Where(r => r.ID == ind).ToList();
            R.Title = RedNaz.Text;
            R.Cost = Convert.ToDecimal(RedPrice.Text);
            R.DurationInSeconds = Convert.ToInt32(RedTime.Text) *60;
            R.Description = RedOpis.Text;
            R.Discount = Convert.ToInt32(RedSale.Text);
            R.MainImagePath = RedPath.Text;
            Classes.BD.EM.Service.AddOrUpdate(R);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Изменено");
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
            int ind = Convert.ToInt32(BtnDel.Uid);
            Service S = ServisList[ind];
            Classes.BD.EM.Service.Remove(S);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Удалено");
            Global.MF.Navigate(new Pages.Admin());

        }

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
            int ind = Convert.ToInt32(BtnNew.Uid);
            Service S = ServisList[ind];
            

        }
        private void IBtn_Click(object sender, RoutedEventArgs e)//кнопка для поиска изображения
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string PathI = OFD.FileName;
            IPath.Text = PathI;
        }
        private void SaveNew_Click(object sender, RoutedEventArgs e)
        {
            Service ObjectServ = new Service()
            {
                Title = Naz.Text,
                Cost = Convert.ToInt32(Price.Text),
                DurationInSeconds = Convert.ToInt32(Time.Text)*60,
                Description = Opis.Text,
                Discount = Convert.ToDouble(Sale.Text)/100,
                MainImagePath = IPath.Text
            };
            Classes.BD.EM.Service.Add(ObjectServ);
            Classes.BD.EM.SaveChanges();
            MessageBox.Show("Услуга добавлена");
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
           
        }

        private void Home_Click(object sender, RoutedEventArgs e)//кнопка возврата
        {

            DGServises.Visibility = Visibility.Visible;
            Redact.Visibility = Visibility.Collapsed;
            BtnYsl.Visibility = Visibility.Visible;
            Global.MF.Navigate(new Pages.Admin());
        }

       
    }
}


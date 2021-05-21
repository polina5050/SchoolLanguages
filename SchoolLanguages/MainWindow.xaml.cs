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

namespace SchoolLanguages
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path;
        public MainWindow()
        {
            InitializeComponent();
            Global.MF = frm;
            Classes.BD.EM = new KrainovaEntities();
            frm.Navigate(new Pages.Admin());
            BitmapImage BMI = new BitmapImage();
            BMI.BeginInit();
            path = @"/Resrs\school_logo.png";
            BMI.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            BMI.EndInit();
            Logo.Source = BMI;
        }
    }
}

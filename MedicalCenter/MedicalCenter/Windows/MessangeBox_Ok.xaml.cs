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
using System.Windows.Shapes;
using System.Configuration;

namespace MedicalCenter.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessangeBox_Ok.xaml
    /// </summary>
    public partial class MessangeBox_Ok : Window
    {
        public MessangeBox_Ok(string messange)
        {
            InitializeComponent();
            Messange.Text = messange;
        }

        private void Оk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

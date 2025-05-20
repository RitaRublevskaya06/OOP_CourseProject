using MedicalCenter.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Configuration; // Для ConfigurationManager
using System.IO;            // Для File

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для AddReviews.xaml
    /// </summary>
    public partial class AddReviews : Window
    {
        Classes.Clients client = new Classes.Clients();
        string connectionString;
        string allowedchar = "'";
        bool top = true;
        bool dec = true;
        public AddReviews(Classes.Clients clients)
        {
            InitializeComponent();
            client = clients;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
        }

        private void Topic_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Topic.Text == "тема")
            {
                Topic.Text = "";
            }
        }

        private void Topic_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Topic.Text))
                Topic.Text = "тема";

            if ((Topic.Text.ToString().Contains(allowedchar)))
            {
                top = false;
                Warring.Text = "Текст содержит заперещенные знаки";
                return;
            }
        }

        private void Decription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Decription.Text == "отзыв")
            {
                Decription.Text = "";
            }

        }

        private void Decription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Decription.Text))
                Decription.Text = "отзыв";

            if ((Decription.Text.ToString().Contains(allowedchar)))
            {
                dec = false;
                Warring.Text = "Текст содержит заперещенные знаки";
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Topic.Text != "тема" && Decription.Text != "отзыв" && top && dec)
            {

                string sql = $"INSERT INTO Reviews ([ID_Client],[Heading],[Review]) VALUES " +
                    $"('{client.ID_Clients}','{Topic.Text}','{Decription.Text}')";
                try
                {

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command1 = new SqlCommand(sql, sqlConnection);
                        command1.ExecuteNonQuery();
                        MessangeBox_Ok messageBox_Ok = new MessangeBox_Ok("Отзыв отправлен!");
                        messageBox_Ok.Show();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (Topic.Text == "тема")
            {
                Warring.Text = "Вы не указали тему";
                return;
            }
            if (Decription.Text == "отзыв")
            {
                Warring.Text = "Вы не написали отзыв";
                return;
            }


        }
    }
}

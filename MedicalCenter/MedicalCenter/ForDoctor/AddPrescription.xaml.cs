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
using System.Configuration; 
using MedicalCenter.Windows;


namespace MedicalCenter.ForDoctor
{
    /// <summary>
    /// Логика взаимодействия для AddPrescription.xaml
    /// </summary>
    public partial class AddPrescription : Window
    {
        private Classes.Doctor _doctor;
        private string _connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;

        public AddPrescription(Classes.Doctor doctor)
        {
            InitializeComponent();
            _doctor = doctor;

            // Инициализация текста через код (можно перенести в XAML)
            ClientId.Text = "ID клиента";
            DoctorId.Text = "ID доктора";
            PrescriptionText.Text = "Текст рецепта";
        }

        private void ClientId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientId.Text == "ID клиента")
                ClientId.Text = "";
        }

        private void ClientId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClientId.Text))
                ClientId.Text = "ID клиента";
        }

        private void DoctorId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DoctorId.Text == "ID доктора")
                DoctorId.Text = "";
        }

        private void DoctorId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DoctorId.Text))
                DoctorId.Text = "ID доктора";
        }

        private void PrescriptionText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PrescriptionText.Text == "Текст рецепта")
                PrescriptionText.Text = "";
        }

        private void PrescriptionText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PrescriptionText.Text))
                PrescriptionText.Text = "Текст рецепта";
        }

        private void SavePrescription_Click(object sender, RoutedEventArgs e)
        {
            string sql = @"
            INSERT INTO Prescriptions (DoctorID, ClientID, Text, Date) 
            VALUES (@DoctorId, @ClientId, @Text, @Date)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DoctorId", _doctor.ID_Doctor);
                command.Parameters.AddWithValue("@ClientId", ClientId.Text);
                command.Parameters.AddWithValue("@Text", PrescriptionText.Text);
                command.Parameters.AddWithValue("@Date", PrescriptionDate.SelectedDate ?? DateTime.Now);

                //try
                //{
                //    connection.Open();
                //    command.ExecuteNonQuery();
                //    MessageBox.Show("Рецепт сохранен!");
                //    this.Close();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show($"Ошибка: {ex.Message}");
                //}
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    MessangeBox_Ok messange = new MessangeBox_Ok("Рецепт сохранен!");
                    messange.ShowDialog();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }

            }
        }
    }
}

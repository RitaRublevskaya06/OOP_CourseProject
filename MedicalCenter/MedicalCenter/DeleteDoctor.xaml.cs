using MedicalCenter.Classes;
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

namespace MedicalCenter
{
    /// <summary>
    /// Логика взаимодействия для DeleteDoctor.xaml
    /// </summary>
    public partial class DeleteDoctor : Window
    {
        string connectionString;
        public DeleteDoctor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            InitializeComponent();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;

                    sqlExpression2 = $"SELECT * FROM Doctor";
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Doctor> doctors = new List<Classes.Doctor>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();

                            doctor.ID_Doctor = reader.GetValue(0);
                            doctor.Name_Doctor = reader.GetValue(1);
                            doctor.Surname_Doctor = reader.GetValue(2);
                            doctor.Middle_name_Doctor = reader.GetValue(3);
                            doctor.Spezialization = reader.GetValue(4);
                            doctor.Study = reader.GetValue(5);
                            doctor.Work_experience = reader.GetValue(6);
                            doctor.Photo_Doctor = reader.GetValue(7);
                            doctors.Add(doctor);
                        }
                    }
                    reader.Close();
                    Grid.ItemsSource = doctors;
                }
                catch
                {

                }
            }
        }

        private void BinDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItems != null)
            {
                int id = (int)(Grid.SelectedItem as Doctor).ID_Doctor;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression1 = $"Delete from schedule where ID_Doctor = '{id}'";
                        string sqlExpression2 = $"Delete from [Order] where ID_Doctor = '{id}'";
                        string sqlExpression3 = $"Delete from Coupon where ID_Doctor = '{id}'";
                        string sqlExpression4 = $"Delete from Doctor where ID_Doctor = '{id}'";
                        SqlCommand command = new SqlCommand(sqlExpression1, connection);
                        command.ExecuteNonQuery();
                        SqlCommand command1 = new SqlCommand(sqlExpression2, connection);
                        command1.ExecuteNonQuery();
                        SqlCommand command2 = new SqlCommand(sqlExpression3, connection);
                        command2.ExecuteNonQuery();
                        SqlCommand command3 = new SqlCommand(sqlExpression4, connection);
                        command3.ExecuteNonQuery();
                    }
                    catch
                    {

                    }

                }

                UpdateDB();
            }
        }

        private void UpdateDB()
        {


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;
                    sqlExpression2 = $"SELECT * FROM Doctor";
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Doctor> doctors = new List<Classes.Doctor>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();

                            doctor.ID_Doctor = reader.GetValue(0);
                            doctor.Name_Doctor = reader.GetValue(1);
                            doctor.Surname_Doctor = reader.GetValue(2);
                            doctor.Middle_name_Doctor = reader.GetValue(3);
                            doctor.Spezialization = reader.GetValue(4);
                            doctor.Study = reader.GetValue(5);
                            doctor.Work_experience = reader.GetValue(6);
                            doctor.Photo_Doctor = reader.GetValue(7);
                            doctors.Add(doctor);
                        }
                    }
                    reader.Close();
                    Grid.ItemsSource = doctors;
                }
                catch
                {

                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {

            UpdateDB();
        }
    }
}

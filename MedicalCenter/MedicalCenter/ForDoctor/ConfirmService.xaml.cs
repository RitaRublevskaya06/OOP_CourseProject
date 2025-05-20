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
using System.IO; 

namespace MedicalCenter.ForDoctor
{
    /// <summary>
    /// Логика взаимодействия для ConfirmService.xaml
    /// </summary>
    public partial class ConfirmService : Window
    {
        string connectionString;
        public ConfirmService()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;
                    string sqlExpression3;
                    // Для sqlExpression2:
                    sqlExpression2 = $@"
                                      SELECT [Order].Order_number, 
                                      [Order].Date, 
                                      [Order].[Time], 
                                      [Order].ID_Coupon, 
                                      [Procedures].Name_Procedures, 
                                      Doctor.Name_Doctor, 
                                      Doctor.Middle_name_Doctor,
                                      Doctor.ID_Doctor, 
                                      [Procedures].Price, 
                                      Clients.Name_Client, 
                                      Clients.Surname_Client,
                                      Clients.ID_Client 
                                      FROM [Order] 
                                      INNER JOIN [Procedures] ON [Order].ID_Procedures = [Procedures].ID_Procedures 
                                      INNER JOIN Doctor ON [Order].ID_Doctor = Doctor.ID_Doctor 
                                      INNER JOIN Clients ON Clients.ID_Client = [Order].ID_Client 
                                      WHERE [Order].Date > CAST(GETDATE() AS DATE)";

                    // Для sqlExpression3:
                    sqlExpression3 = $@"
                                      SELECT [Order].Order_number, 
                                      [Order].Date, 
                                      [Order].[Time], 
                                      [Order].ID_Coupon,
                                      [Procedures].Name_Procedures, 
                                      Doctor.Name_Doctor, 
                                      Doctor.Middle_name_Doctor,
                                      Doctor.ID_Doctor, 
                                      [Procedures].Price, 
                                      Clients.Name_Client, 
                                      Clients.Surname_Client,
                                      Clients.ID_Client 
                                      FROM [Order] 
                                      INNER JOIN [Procedures] ON [Order].ID_Procedures = [Procedures].ID_Procedures 
                                      INNER JOIN Doctor ON [Order].ID_Doctor = Doctor.ID_Doctor 
                                      INNER JOIN Clients ON Clients.ID_Client = [Order].ID_Client 
                                      WHERE [Order].Date = CAST(GETDATE() AS DATE) 
                                      AND [Order].Time > CAST(GETDATE() AS TIME)";

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Order> orders = new List<Classes.Order>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Order order = new Classes.Order
                            {
                                Order_number = reader.GetInt32(0),
                                Date = ((DateTime)reader["Date"]).ToShortDateString(),
                                Time = reader["Time"].ToString(),
                                ID_Coupon = reader["ID_Coupon"],
                                Name_Procedures = reader["Name_Procedures"].ToString(),
                                ID_Doctor = $"{reader["Name_Doctor"]} {reader["Middle_name_Doctor"]}",
                                ID_DoctorValue = Convert.ToInt32(reader["ID_Doctor"]),
                                ID_Client = $"{reader["Name_Client"]} {reader["Surname_Client"]}",
                                ID_ClientValue = Convert.ToInt32(reader["ID_Client"]),
                                Price = reader["Price"]
                            };

                            orders.Add(order);
                        }
                    }
                    reader.Close();
                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            Classes.Order order = new Classes.Order();
                            order.Order_number = reader1.GetValue(0);
                            DateTime a = (DateTime)reader1.GetValue(1);
                            order.Date = a.ToShortDateString();
                            order.Time = reader1.GetValue(2);
                            order.ID_Coupon = reader1.GetValue(3);
                            order.ID_Procedures = reader1.GetValue(4);
                            string name = (string)reader1.GetValue(5);
                            string surname = (string)reader1.GetValue(6);
                            order.ID_Doctor = $"{name} {surname}";
                            string clientname = (string)reader1.GetValue(10);
                            string clientsurname = (string)reader1.GetValue(11);
                            order.ID_Client = $"{clientname} {clientsurname}";
                            order.Price = reader1.GetValue(9);

                            orders.Add(order);
                        }
                    }
                    reader1.Close();

                    //Control.ItemsSource = orders;
                    OrdersGrid.ItemsSource = orders;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private bool _isDoctor = false;
        private int _currentDoctorId;

        // Конструктор для врача
        public ConfirmService(Doctor doctor)
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            _isDoctor = true;
            _currentDoctorId = Convert.ToInt32(doctor.ID_Doctor); // Приведение типа
            LoadOrdersForDoctor(_currentDoctorId);
            HideAdminControls();
        }
        // Загрузка заказов для врача
        private void LoadOrdersForDoctor(object doctorId)
        {
            string sql = @"SELECT o.Order_number, o.ID_Client, o.ID_Doctor, o.Date, o.Time, o.ID_Coupon, 
                      p.Name_Procedures, d.Name_Doctor, d.Middle_name_Doctor, 
                      p.Price, c.Name_Client, c.Surname_Client
               FROM [Order] o
               INNER JOIN [Procedures] p ON o.ID_Procedures = p.ID_Procedures
               INNER JOIN [Doctor] d ON o.ID_Doctor = d.ID_Doctor
               INNER JOIN Clients c ON o.ID_Client = c.ID_Client
               WHERE o.ID_Doctor = @DoctorID 
               AND o.Status = 'Pending'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                SqlDataReader reader = cmd.ExecuteReader();

                List<Classes.Order> orders = new List<Classes.Order>();
                while (reader.Read())
                {
                    Classes.Order order = new Classes.Order
                    {
                        Order_number = reader["Order_number"].ToString(), // Приведение к строке
                        Date = ((DateTime)reader["Date"]).ToShortDateString(),
                        Time = reader["Time"].ToString(),
                        ID_Coupon = reader["ID_Coupon"].ToString(), // Приведение к строке
                        Name_Procedures = reader["Name_Procedures"].ToString(),
                        ID_DoctorValue = Convert.ToInt32(reader["ID_Doctor"]), // Преобразование к int
                        ID_ClientValue = Convert.ToInt32(reader["ID_Client"]), // Преобразование к int
                        ID_Doctor = $"{reader["Name_Doctor"]} {reader["Middle_name_Doctor"]}",
                        ID_Client = $"{reader["Name_Client"]} {reader["Surname_Client"]}",
                        Price = Convert.ToDecimal(reader["Price"]) // Преобразование к decimal, если необходимо
                    };
                    orders.Add(order);
                }
                OrdersGrid.ItemsSource = orders;
            }
        }

        // Скрыть кнопки администратора
        private void HideAdminControls()
        {
            //AddTalon.Visibility = Visibility.Collapsed;
            //Delete.Visibility = Visibility.Collapsed;
            //Add.Visibility = Visibility.Collapsed;
        }

        // Подтверждение заказа
        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!_isDoctor) return;

            var order = (Order)((Button)sender).DataContext;
            string sql = "UPDATE [Order] SET Status = 'Completed' WHERE Order_number = @OrderID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderID", order.Order_number);
                cmd.ExecuteNonQuery();
                LoadOrdersForDoctor((int)_currentDoctorId); // Обновить список
            }
        }
        private void ConfirmService_Click(object sender, RoutedEventArgs e)
        {
            if (!_isDoctor) return;

            var order = (Order)((Button)sender).DataContext;
            string sql = "UPDATE [Order] SET Status = 'Completed' WHERE Order_number = @OrderID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderID", order.Order_number);
                cmd.ExecuteNonQuery();
                LoadOrdersForDoctor((int)_currentDoctorId);
            }
        }


        private void CancelService_Click(object sender, RoutedEventArgs e)
        {
            if (!_isDoctor) return;

            var order = (Order)((Button)sender).DataContext;
            string sql = "UPDATE [Order] SET Status = 'Canceled' WHERE Order_number = @OrderID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@OrderID", order.Order_number);
                cmd.ExecuteNonQuery();
                LoadOrdersForDoctor((int)_currentDoctorId);
            }
        }

    }
}

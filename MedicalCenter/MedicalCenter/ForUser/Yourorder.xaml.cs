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

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для Yourorder.xaml
    /// </summary>
    public partial class Yourorder : Window
    {
        string connectionString;
        Classes.Clients client;

        public Yourorder(Classes.Clients clients)
        {
            InitializeComponent();
            client = clients;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;
                    string sqlExpression3;
                    sqlExpression2 = $"SELECT [Order].Order_number,[Order].Date,[Order].[Time],[Order].[ID_Procedures],[Order].ID_Client,[Order].ID_Doctor,[Order].ID_Coupon, [Order].Status, [Procedures].Name_Procedures,Doctor.Name_Doctor,Doctor.Middle_name_Doctor,[Procedures].Price FROM [Order] " +
                        $"inner join[Procedures] on[Order].ID_Procedures =[Procedures].ID_Procedures and ID_Client ={client.ID_Clients} and Date>cast(GETDATE() as date)" +
                        $"inner join[Doctor] on[Order].ID_Doctor =[Doctor].ID_Doctor";

                    sqlExpression3 = $"SELECT [Order].Order_number,[Order].Date,[Order].[Time],[Order].[ID_Procedures],[Order].ID_Client,[Order].ID_Doctor,[Order].ID_Coupon, [Order].Status, [Procedures].Name_Procedures,Doctor.Name_Doctor,Doctor.Middle_name_Doctor,[Procedures].Price FROM [Order] " +
                        $"inner join[Procedures] on[Order].ID_Procedures =[Procedures].ID_Procedures and ID_Client ={client.ID_Clients} and Date=cast(GETDATE() as date) and Time>CAST(GETDATE() as time)" +
                        $"inner join[Doctor] on[Order].ID_Doctor =[Doctor].ID_Doctor";

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Order> orders = new List<Classes.Order>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Order order = new Classes.Order();

                            order.Order_number = reader.GetValue(0);
                            DateTime a = (DateTime)reader.GetValue(1);
                            order.Date = a.ToShortDateString();
                            order.Time = reader.GetValue(2);
                            order.ID_Procedures = reader.GetValue(3);
                            order.ID_Client = reader.GetValue(4);
                            order.ID_Doctor = reader.GetValue(5);
                            order.ID_Coupon = reader.GetValue(6);
                            order.Status = reader["Status"].ToString();
                            order.Name_Procedures = reader.GetValue(7);
                            order.Name_Doctor = reader.GetValue(8);
                            order.Middle_name_Doctor = reader.GetValue(9);
                            order.Price = reader.GetValue(10);
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
                            order.Date = reader1.GetValue(1);
                            DateTime a = (DateTime)reader1.GetValue(2);
                            order.Time = a.ToShortDateString();
                            order.ID_Procedures = reader1.GetValue(3);
                            order.ID_Client = reader1.GetValue(4);
                            order.ID_Doctor = reader1.GetValue(5);
                            order.ID_Coupon = reader1.GetValue(6);
                            order.Name_Procedures = reader1.GetValue(7);
                            order.Name_Doctor = reader1.GetValue(8);
                            order.Middle_name_Doctor = reader1.GetValue(9);
                            order.Price = reader1.GetValue(10);

                            orders.Add(order);
                        }
                    }
                    Control.ItemsSource = orders;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace MedicalCenter
{
    /// <summary>
    /// Логика взаимодействия для AllOrder.xaml
    /// </summary>
    public partial class AllOrder : Page
    {
        string connectionString;
        public AllOrder()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            //connectionString = ConfigurationManager.ConnectionStrings["Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;
                    string sqlExpression3;
                    sqlExpression2 = $"SELECT [Order].Order_number,[Order].Date,[Order].[Time],[Order].ID_Coupon,[Order].Status,[Procedures].Name_Procedures,Doctor.Name_Doctor,Doctor.Middle_name_Doctor,[Procedures].Price, Clients.Name_Client,Clients.Surname_Client FROM [Order] " +
                        $"inner join[Procedures] on[Order].ID_Procedures =[Procedures].ID_Procedures and Date> cast(GETDATE() as date)" +
                        $"inner join[Doctor] on[Order].ID_Doctor =[Doctor].ID_Doctor " +
                        $"inner join Clients on Clients.ID_Client =[Order].ID_Client";

                    sqlExpression3 = $"SELECT [Order].Order_number,[Order].Date,[Order].[Time],[Order].ID_Coupon,[Order].Status,[Procedures].Name_Procedures,Doctor.Name_Doctor,Doctor.Middle_name_Doctor,[Procedures].Price, Clients.Name_Client,Clients.Surname_Client FROM [Order] " +
                        $"inner join[Procedures] on[Order].ID_Procedures =[Procedures].ID_Procedures  and Date = cast(GETDATE() as date) and Time> CAST(GETDATE() as time)" +
                        $"inner join[Doctor] on[Order].ID_Doctor =[Doctor].ID_Doctor " +
                        $"inner join Clients on Clients.ID_Client =[Order].ID_Client";

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
                            order.ID_Coupon = reader.GetValue(3);
                            order.Status = reader.GetValue(4); // ✔ статус

                            order.ID_Procedures = reader.GetValue(5); // Name_Procedures
                            string name = (string)reader.GetValue(6); // Name_Doctor
                            string surname = (string)reader.GetValue(7); // Middle_name_Doctor
                            order.ID_Doctor = $"{name} {surname}";
                            order.Price = reader.GetValue(8);

                            string clientname = (string)reader.GetValue(9);
                            string clientsurname = (string)reader.GetValue(10);
                            order.ID_Client = $"{clientname} {clientsurname}";

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
                            order.Status = reader1.GetValue(4);
                            order.ID_Procedures = reader1.GetValue(5);
                            string name = (string)reader1.GetValue(6);
                            string surname = (string)reader1.GetValue(7);
                            order.ID_Doctor = $"{name} {surname}";
                            order.Price = reader1.GetValue(8);
                            string clientname = (string)reader1.GetValue(9);
                            string clientsurname = (string)reader1.GetValue(10);
                            order.ID_Client = $"{clientname} {clientsurname}";



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
    }
}

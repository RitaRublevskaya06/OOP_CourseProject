using MedicalCenter.Classes;
using MedicalCenter.ForAdmin;
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
    /// Логика взаимодействия для Deleteprosedurec.xaml
    /// </summary>
    public partial class Deleteprosedurec : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        public Deleteprosedurec()
        {
            InitializeComponent();
            Grid.CanUserAddRows = false;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {

                    string sqlExpression2 = $"SELECT * FROM Procedures";
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Procedures> procedures = new List<Classes.Procedures>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Procedures procedure = new Classes.Procedures();

                            procedure.ID_Procedures = reader.GetValue(0);
                            procedure.Name_Procedures = reader.GetValue(1);
                            procedure.Decription = reader.GetValue(2);
                            procedure.Price = reader.GetValue(3);
                            procedure.Photo = reader.GetValue(4);
                            procedure.Spezialization = reader.GetValue(5);

                            procedures.Add(procedure);
                        }
                    }
                    reader.Close();
                    Grid.ItemsSource = procedures;
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
                int id = (int)(Grid.SelectedItem as Procedures).ID_Procedures;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression1 = $"DELETE FROM [Order] where ID_Procedures='{id}'";
                        string sqlExpression2 = $"DELETE FROM [Procedures] where ID_Procedures='{id}'";
                        SqlCommand command = new SqlCommand(sqlExpression1, connection);
                        command.ExecuteNonQuery();
                        SqlCommand command1 = new SqlCommand(sqlExpression2, connection);
                        command1.ExecuteNonQuery();

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

                    string sqlExpression2 = $"SELECT * FROM Procedures";
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Procedures> procedures = new List<Classes.Procedures>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Procedures procedure = new Classes.Procedures();

                            procedure.ID_Procedures = reader.GetValue(0);
                            procedure.Name_Procedures = reader.GetValue(1);
                            procedure.Decription = reader.GetValue(2);
                            procedure.Price = reader.GetValue(3);
                            procedure.Photo = reader.GetValue(4);
                            procedure.Spezialization = reader.GetValue(5);

                            procedures.Add(procedure);
                        }
                    }
                    reader.Close();
                    Grid.ItemsSource = procedures;
                }
                catch
                {

                }
            }
        }

        private void BinEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItems != null)
            {
                int id = (int)(Grid.SelectedItem as Procedures).ID_Procedures;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression1 = $"Select * from [Procedures] where ID_Procedures='{id}'";
                        SqlCommand command = new SqlCommand(sqlExpression1, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        Classes.Procedures procedure = new Classes.Procedures();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {


                                procedure.ID_Procedures = reader.GetValue(0);
                                procedure.Name_Procedures = reader.GetValue(1);
                                procedure.Decription = reader.GetValue(2);
                                procedure.Price = reader.GetValue(3);
                                procedure.Photo = reader.GetValue(4);
                                procedure.Photo_Procedures_byte = (byte[])reader.GetValue(4);
                                procedure.Spezialization = reader.GetValue(5);


                            }
                        }
                        reader.Close();
                        RegistrationofProcedures registrationofProcedures = new RegistrationofProcedures(procedure);
                        registrationofProcedures.Show();
                        this.Close();

                    }
                    catch
                    {

                    }

                }

                UpdateDB();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateDB();
        }
    }
}

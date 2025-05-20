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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Configuration;

namespace MedicalCenter.Windows
{
    /// <summary>
    /// Логика взаимодействия для allprocedures.xaml
    /// </summary>
    public partial class allprocedures : Page
    {
        string connectionString;
        public List<Classes.Doctor> doctors = new List<Classes.Doctor>();
        public allprocedures()
        {

            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            Doctorname.SelectedIndex = 0;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            Add.Width = 0; Add.Height = 0;
            DelEdit.Width = 0; DelEdit.Height = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT * FROM Doctor ";
                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();
                            doctor.ID_Doctor = reader.GetValue(0);
                            doctor.Name_Doctor = reader.GetValue(1);
                            doctor.Surname_Doctor = reader.GetValue(2);
                            doctor.Middle_name_Doctor = reader.GetValue(3);
                            doctors.Add(doctor);

                            comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                            Doctorname.Items.Add(comdobox);
                        }
                    }
                }
                catch
                {

                }
            }

        }

        public allprocedures(ForAdmin.Admin admin)
        {
            InitializeComponent();
            Doctorname.SelectedIndex = 0;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT * FROM Doctor ";
                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();

                            doctor.ID_Doctor = reader.GetValue(0);
                            doctor.Name_Doctor = reader.GetValue(1);
                            doctor.Surname_Doctor = reader.GetValue(2);
                            doctor.Middle_name_Doctor = reader.GetValue(3);
                            doctors.Add(doctor);

                            comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                            Doctorname.Items.Add(comdobox);

                        }
                    }
                    reader.Close();
                    connection.Close();
                }
                catch
                {

                }
            }

        }

        private void Add_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationofProcedures window1 = new RegistrationofProcedures();
            window1.Show();
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = null;
                    if (Doctorname.Text == "Врач")
                        sqlExpression2 = $"SELECT * FROM Procedures";
                    else
                    {

                        foreach (var doc in doctors)
                        {
                            if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == Doctorname.Text)
                            {
                                sqlExpression2 = $"SELECT [Procedures].ID_Procedures,[Procedures].Name_Procedures,[Procedures].Decription,[Procedures].Price,[Procedures].Photo FROM [Procedures] " +
                                $"inner join Doctor on [Procedures].Spezialization=Doctor.Spezialization and Doctor.ID_Doctor={doc.ID_Doctor}";
                                break;
                            }
                            else
                                sqlExpression2 = $"SELECT ID_Procedures FROM Procedures";
                        }

                    }

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
                            // procedure.Spezialization = reader.GetValue(5);

                            procedures.Add(procedure);
                        }
                    }
                    reader.Close();
                    Control.ItemsSource = procedures;
                }
                catch
                {

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Deleteprosedurec deleteprosedurec = new Deleteprosedurec();
            deleteprosedurec.Show();
        }
    }
}

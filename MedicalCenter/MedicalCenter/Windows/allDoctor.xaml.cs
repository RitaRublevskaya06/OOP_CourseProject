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
    /// Логика взаимодействия для allDoctor.xaml
    /// </summary>
    public partial class allDoctor : Page
    {
        static public bool messange;
        public static object names;
        public bool Admin = false;
        public bool guest = false;
        public bool user = false;
        string connectionString;
        public allDoctor()
        {

            InitializeComponent();
            guest = true;
            Spezializationname.SelectedIndex = 0;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            AddTalon.Width = 0;
            AddTalon.Height = 0;
            Delete.Height = 0;
            Delete.Width = 0;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            Add.Width = 0; Add.Height = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT * FROM Spezialization ";
                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Spezialization> spezializations = new List<Classes.Spezialization>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Spezialization spezialization = new Classes.Spezialization();

                            spezialization.Spezializationname = reader.GetValue(0);

                            comdobox = (string)spezialization.Spezializationname;
                            Spezializationname.Items.Add(comdobox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // Добавьте этот конструктор
        public allDoctor(Classes.Doctor doctor)
        {
            InitializeComponent(); // Это критически важно для инициализации элементов XAML
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;

            // Скрываем элементы управления
            Delete.Visibility = Visibility.Collapsed;
            AddTalon.Visibility = Visibility.Collapsed;
            Add.Visibility = Visibility.Collapsed;

            Spezializationname.SelectedIndex = 0;
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

                    string sqlExpression2 = $"SELECT * FROM Spezialization ";

                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Spezialization> spezializations = new List<Classes.Spezialization>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Spezialization spezialization = new Classes.Spezialization();

                            spezialization.Spezializationname = reader.GetValue(0);

                            comdobox = (string)spezialization.Spezializationname;
                            Spezializationname.Items.Add(comdobox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        public allDoctor(Classes.Clients clients)
        {
            InitializeComponent();
            Spezializationname.SelectedIndex = 0;
            user = true;
            AddTalon.Width = 0;
            AddTalon.Height = 0;
            Delete.Height = 0;
            Delete.Width = 0;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            Add.Width = 0; Add.Height = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT * FROM Spezialization ";
                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Spezialization> spezializations = new List<Classes.Spezialization>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Spezialization spezialization = new Classes.Spezialization();

                            spezialization.Spezializationname = reader.GetValue(0);

                            comdobox = (string)spezialization.Spezializationname;
                            Spezializationname.Items.Add(comdobox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public allDoctor(ForAdmin.Admin admin)
        {

            InitializeComponent();
            Admin = true;
            Spezializationname.SelectedIndex = 0;
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

                    string sqlExpression2 = $"SELECT * FROM Spezialization ";

                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Spezialization> spezializations = new List<Classes.Spezialization>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Spezialization spezialization = new Classes.Spezialization();

                            spezialization.Spezializationname = reader.GetValue(0);

                            comdobox = (string)spezialization.Spezializationname;
                            Spezializationname.Items.Add(comdobox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void Add_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistrationofDoctor window1 = new RegistrationofDoctor();
            window1.Show();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2;
                    if (Spezializationname.Text == "Специальность")
                    {
                        sqlExpression2 = $"SELECT * FROM Doctor";
                    }
                    else
                    { sqlExpression2 = $"SELECT * FROM Doctor Where Spezialization='{Spezializationname.Text}'"; }

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Doctor> doctors = new List<Classes.Doctor>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();

                            doctor.ID_Doctor = reader.GetValue(0);
                            doctor.Name_Doctor = (string)reader.GetValue(1);
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
                    Control.ItemsSource = doctors;
                }
                catch
                {

                }
            }
        }

        private void AddTalon_Click(object sender, RoutedEventArgs e)
        {
            AddTalon add = new AddTalon();
            add.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteDoctor deleteDoctor = new DeleteDoctor();
            deleteDoctor.Show();
        }
    }
}

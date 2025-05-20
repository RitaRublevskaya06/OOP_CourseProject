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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.IO;

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        string connectionString;
        public Login()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "пароль")
            {
                Password.Text = "";
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Text))
                Password.Text = "пароль";
        }

        private void Login_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Logine.Text == "логин")
            {
                Logine.Text = "";
            }
        }

        private void Login_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Logine.Text))
                Logine.Text = "логин";
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new Registration());
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка администратора
            bool pointadmin = false;
            ForAdmin.Admin admin = new ForAdmin.Admin();
            if (Logine.Text == admin.login && Password.Text == admin.password)
            {
                MainHarmony window1 = new MainHarmony(admin);
                window1.Show();
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.Close();
                        pointadmin = true;
                        break;
                    }
                }
            }
            // 2. Проверка врача
            string doctorSql = "SELECT * FROM Doctor WHERE DoctorLogin = @Login AND DoctorPassword = @Password";
            using (SqlConnection doctorConnection = new SqlConnection(connectionString))
            {
                doctorConnection.Open();
                SqlCommand doctorCommand = new SqlCommand(doctorSql, doctorConnection);
                doctorCommand.Parameters.AddWithValue("@Login", Logine.Text);
                doctorCommand.Parameters.AddWithValue("@Password", Password.Text);

                using (SqlDataReader doctorReader = doctorCommand.ExecuteReader())
                {
                    if (doctorReader.HasRows)
                    {
                        Classes.Doctor doctor = new Classes.Doctor();
                        while (doctorReader.Read())
                        {
                            doctor.ID_Doctor = doctorReader["ID_Doctor"];
                            doctor.Name_Doctor = doctorReader["Name_Doctor"].ToString();
                            doctor.Surname_Doctor = doctorReader["Surname_Doctor"].ToString();
                            doctor.DoctorLogin = doctorReader["DoctorLogin"].ToString();
                            doctor.DoctorPassword = doctorReader["DoctorPassword"].ToString();
                        }
                        MainHarmony window1 = new MainHarmony(doctor);
                        window1.Show();
                        Application.Current.Windows[0].Close();
                        return;
                    }
                }
            }
            // 3. Проверка клиента
            string sql = "SELECT * FROM Clients WHERE Login = @Login AND Password = @Password"; // Используйте параметризованный запрос
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    if (Logine.Text != "логин" && Password.Text != "пароль")
                    {
                        SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@Login", Logine.Text);
                        sqlCommand.Parameters.AddWithValue("@Password", Password.Text);

                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            Classes.Clients clients = new Classes.Clients();
                            if (reader.HasRows)
                            {
                                bool flagClients = false;
                                while (reader.Read())
                                {
                                    // Проверка пароля и логина
                                    if (Logine.Text == reader["Login"].ToString() && Password.Text == reader["Password"].ToString())
                                    {
                                        flagClients = true;
                                        clients.ID_Clients = reader["ID_Client"];
                                        clients.Name_Client = reader["Name_Client"].ToString();
                                        clients.Surname_Client = reader["Surname_Client"].ToString();
                                        clients.Email = reader["Email"].ToString();
                                        clients.Password = reader["Password"].ToString();
                                        clients.Login = reader["Login"].ToString();

                                        // Обработка изображения
                                        if (reader["Image"] != DBNull.Value)
                                        {
                                            clients.Image = (byte[])reader["Image"];
                                        }
                                        else
                                        {
                                            clients.Image = GetDefaultImageBytes();
                                        }

                                        break;
                                    }
                                }
                                reader.Close();

                                if (flagClients)
                                {
                                    MainHarmony window1 = new MainHarmony(clients);
                                    window1.Show();
                                    foreach (Window window in Application.Current.Windows)
                                    {
                                        if (window is MainWindow)
                                        {
                                            window.Close();
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!pointadmin)
                                        Warring.Text = "Неправильно введен логин или пароль";
                                }
                            }
                        }
                    }
                    else
                    {
                        Warring.Text = "Данные до конца не введены";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private byte[] GetDefaultImageBytes()
        {
            try
            {
                string defaultImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "user.png");
                if (File.Exists(defaultImagePath))
                {
                    return File.ReadAllBytes(defaultImagePath);
                }
                return new byte[0]; // Пустой массив, если файла нет
            }
            catch
            {
                return new byte[0]; // Пустой массив в случае ошибки
            }
        }
    }
}

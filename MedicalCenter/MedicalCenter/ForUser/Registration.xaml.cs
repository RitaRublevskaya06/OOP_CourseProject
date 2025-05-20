using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using MedicalCenter.Windows;

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;
        public bool emailbool = true;
        public bool loginregistr = false;
        string connectionString;
        bool names = true;
        bool surnames = true;
        bool password = true;
        string allowedchar = "'";
        public Registration()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
        }

        private void email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (email.Text == "электронная почта")
            {
                email.Text = "";
            }
        }

        private void email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email.Text))
                email.Text = "электронная почта";
            string emails = email.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emails);
            if ((match.Success))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Yesornoemail.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                emailbool = true;
            }
            if ((match.Success == false))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Yesornoemail.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                emailbool = false;
            }
        }

        private void name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text))
                name.Text = "имя";

            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(name.Text))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Yesornoname.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                names = false;
            }
            else if (regex.IsMatch(name.Text))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Yesornoname.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                names = true;
            }
        }

        private void name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (name.Text == "имя")
            {
                name.Text = "";
            }
        }

        private void Logine_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Logine.Text == "логин")
            {
                Logine.Text = "";
            }
        }
        private void Logine_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Logine.Text))
            {
                Logine.Text = "логин";
                return;
            }

            if (Logine.Text.Contains(allowedchar))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Yesornologine.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                loginregistr = false;
                return;
            }

            string sql = "SELECT Login FROM Clients WHERE Login = @Login";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Login", Logine.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows || Logine.Text == "admin")
                            {
                                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                                Yesornologine.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                                loginregistr = false;
                            }
                            else
                            {
                                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                                Yesornologine.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                                loginregistr = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки логина: {ex.Message}");
            }
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

            if ((Password.Text.ToString().Contains(allowedchar)))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Yesornopassword.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                password = false;
                return;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new Login());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Проверка всех условий
            if (!emailbool)
            {
                MessageBox.Show("Некорректный email!");
                return;
            }
            if (!loginregistr)
            {
                MessageBox.Show("Логин уже занят или содержит запрещённые символы!");
                return;
            }
            if (!password || Password.Text.Length < 6)
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов и не содержать апостроф!");
                return;
            }
            if (!names || !surnames)
            {
                MessageBox.Show("Имя и фамилия должны быть на кириллице!");
                return;
            }

            // Загрузка изображения (используйте относительные пути)
            byte[] imageBytes;
            try
            {
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", "woman.png");
                imageBytes = File.ReadAllBytes(imagePath);
            }
            catch
            {
                imageBytes = null; // или установите изображение по умолчанию
            }

            // Параметризованный запрос
            string sql = @"
    INSERT INTO Clients 
        (Name_Client, Surname_Client, Email, Login, Password, Image) 
    VALUES 
        (@Name, @Surname, @Email, @Login, @Password, @Image)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name.Text;
                        command.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = Surrename.Text;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email.Text;
                        command.Parameters.Add("@Login", SqlDbType.NVarChar).Value = Logine.Text;
                        command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password.Text;
                        command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = imageBytes != null ? imageBytes : (object)DBNull.Value;
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            //MessageBox.Show("Регистрация успешна!");
                            //Manager.MainFrame.Navigate(new Login());
                            MessangeBox_Ok messange = new MessangeBox_Ok("Регистрация успешна!");
                            messange.Show();
                            Manager.MainFrame.Navigate(new Login());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Surrename_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Surrename.Text == "фамилия")
            {
                Surrename.Text = "";
            }
        }

        private void Surrename_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Surrename.Text))
                Surrename.Text = "фамилия";

            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(Surrename.Text))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Yesornolastname.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                surnames = false;
            }
            else if (regex.IsMatch(Surrename.Text))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Yesornolastname.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                surnames = true;
            }
        }
    }
}

using MedicalCenter.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
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
using System.Configuration; // Для ConfigurationManager

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для Profil.xaml
    /// </summary>
    public partial class Profil : Page
    {
        Classes.Clients clients;
        string connectionString;
        public byte[] imageBytes;
        public bool emailbool = true;
        bool names = true;
        bool surnames = true;

        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;

        public Profil(Classes.Clients client)
        {
            this.clients = client;
            InitializeComponent();

            // Загрузка строки подключения с проверкой
            var connStringSettings = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"];
            if (connStringSettings == null)
            {
                MessageBox.Show("Ошибка конфигурации: не найдена строка подключения.");
                return;
            }
            connectionString = connStringSettings.ConnectionString;

            LoadPrescriptions();


            Name.Text = $"{client.Name_Client}";
            Surname.Text = $"{client.Surname_Client}";
            Email.Text = $"{client.Email}";
            ////connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            //connectionString = ConfigurationManager.ConnectionStrings["Corser_projectConnectionString"].ConnectionString;
             
            if (client.Image != null)
            {

                var image = new BitmapImage();


                using (var mem = new MemoryStream((client.Image)))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                face.Source = image;

            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Text == "Введите нынешний")
            {
                Password.Text = "";
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password.Text))
                Password.Text = "Введите нынешний";
        }

        private void newPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (newPassword.Text == "Введите новый")
            {
                newPassword.Text = "";
            }
        }

        private void newPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newPassword.Text))
                newPassword.Text = "Введите новый";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)clients.Password == Password.Text)
            {
                if (Password.Text != newPassword.Text)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connection;
                            // Используем параметры вместо прямой вставки значений
                            command.CommandText = @"UPDATE Clients 
                                           SET Password = @NewPassword 
                                           WHERE ID_Client = @ClientID";
                            command.Parameters.AddWithValue("@NewPassword", newPassword.Text);
                            command.Parameters.AddWithValue("@ClientID", clients.ID_Clients);
                            command.ExecuteNonQuery();
                            clients.Password = newPassword.Text;
                            MessangeBox_Ok messange = new MessangeBox_Ok("Данные изменены");
                            messange.Show();
                            newPassword.Text = "Введите новый";
                            Password.Text = "Введите нынешний";
                            Warring.Text = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }


                }
                else
                {
                    Warring.Text = "Вы ввели два одинаковых пароля";

                }

            }
            else if ((string)clients.Password != Password.Text)
            {
                Warring.Text = "Вы ввели неверный нынешний пароль";
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Name.IsReadOnly = false;
            Surname.IsReadOnly = false;
            Email.IsReadOnly = false;

        }
        private void Acceptchanges_Click(object sender, RoutedEventArgs e)
        {
            if (emailbool)
            {
                if (((Name.Text != (string)clients.Name_Client) || (Surname.Text != (string)clients.Surname_Client) || (Email.Text != (string)clients.Email)) && names == true && surnames == true)
                {

                    Name.IsReadOnly = true;
                    Surname.IsReadOnly = true;
                    Email.IsReadOnly = true;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connection;
                            command.CommandText = $@"UPDATE Clients SET Name_Client='{Name.Text}', Surname_Client= '{Surname.Text}', Email='{Email.Text}' WHERE ID_Client = {clients.ID_Clients}";
                            command.ExecuteNonQuery();
                            clients.Password = newPassword.Text;
                            MessangeBox_Ok messange = new MessangeBox_Ok("Данные изменены");
                            messange.Show();
                            clients.Name_Client = Name.Text;
                            clients.Surname_Client = Surname.Text;
                            clients.Email = Email.Text;
                            Check.Source = null;
                            Check1.Source = null;
                            Check2.Source = null;


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string path;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                path = openFile.FileName;
                byte[] imageBytesBuffer;

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    imageBytesBuffer = new byte[fs.Length];
                    fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                }
                imageBytes = imageBytesBuffer;
                clients.Image = imageBytes;
                face.Source = new BitmapImage(new Uri(openFile.FileName));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = $@"UPDATE Clients SET Image = @bytes WHERE ID_Client = {clients.ID_Clients}";
                    command.Parameters.AddWithValue("@bytes", imageBytes);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



        }
        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string imagePath = System.IO.Path.Combine(_basePath, "image", "woman.png");

            if (File.Exists(imagePath))
            {
                BitmapImage defaultImage = new BitmapImage(new Uri(imagePath));
                face.Source = defaultImage;
            }

            clients.Image = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = $@"UPDATE Clients SET Image = NULL WHERE ID_Client = {clients.ID_Clients}";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            string emails = Email.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emails);
            if ((match.Success) && (Email.Text != "электронная почта"))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Check.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                emailbool = true;
            }
            if ((match.Success == false) && (Email.Text != "электронная почта"))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Check.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                emailbool = false;
            }
        }

        private void Ord_Click(object sender, RoutedEventArgs e)
        {
            Yourorder yourorder = new Yourorder(clients);
            yourorder.Show();
            LoadPrescriptions();
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
                Name.Text = "имя";

            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(Name.Text))
            {
                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Check2.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            else if (regex.IsMatch(Name.Text))
            {

                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Check2.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                names = true;
            }
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "имя")
            {
                Name.Text = "";
            }
        }

        private void Surname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Surname.Text))
                Surname.Text = "фамилия";

            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(Surname.Text))
            {

                string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                Check1.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                surnames = false;
            }
            else if (regex.IsMatch(Surname.Text))
            {

                string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                Check1.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                surnames = true;
            }
        }

        private void Surname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Surname.Text == "фамилия")
            {
                Surname.Text = "";
            }
        }
        //private void LoadPrescriptions()
        //{
        //    //string sql = @"SELECT p.Text, p.Date, d.Name_Doctor 
        //    //       FROM Prescriptions p
        //    //       JOIN Doctor d ON p.DoctorID = d.ID_Doctor
        //    //       WHERE p.ClientID = @ClientID";
        //    string sql = @"SELECT p.Text AS [Текст рецепта], 
        //                  p.Date AS [Дата назначения], 
        //                  d.Name_Doctor AS [Врач]
        //           FROM Prescriptions p
        //           JOIN Doctor d ON p.DoctorID = d.ID_Doctor
        //           WHERE p.ClientID = @ClientID";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand(sql, connection);
        //        cmd.Parameters.AddWithValue("@ClientID", clients.ID_Clients);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        adapter.Fill(dt);
        //        PrescriptionsDataGrid.ItemsSource = dt.DefaultView;
        //    }
        //}
        private void LoadPrescriptions()
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Строка подключения не инициализирована.");
                    return;
                }

                string sql = @"SELECT p.Text AS PrescriptionText, 
                      p.Date AS PrescriptionDate, 
                      d.Name_Doctor AS DoctorName
               FROM Prescriptions p
               JOIN Doctor d ON p.DoctorID = d.ID_Doctor
               WHERE p.ClientID = @ClientID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@ClientID", clients.ID_Clients);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    //// Отладочный вывод
                    //MessageBox.Show($"Загружено строк: {dt.Rows.Count}");

                    PrescriptionsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рецептов: {ex.Message}");
            }
        }
    }
}

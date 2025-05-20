using MedicalCenter.Classes;
using MedicalCenter.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Configuration; // Для ConfigurationManager
using System.IO;            // Для File

namespace MedicalCenter.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для RegistrationofProcedures.xaml
    /// </summary>
    public partial class RegistrationofProcedures : Window
    {
        public byte[] imageBytes;
        string connectionString;
        bool names = true;
        bool decrp = true;
        bool price = true;
        bool name = true;
        string allowedchar = "'";
        Classes.Procedures procedurenew = new Procedures();

        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;

        public RegistrationofProcedures()
        {
            InitializeComponent();
            edit.Height = 0;
            edit.Width = 0;
            exit.Height = 0;
            exit.Width = 0;
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
                            Spezialization.Items.Add(comdobox);
                        }
                    }
                    Spezialization.SelectedIndex = 0;
                }
                catch
                {

                }
            }
        }
        public RegistrationofProcedures(Procedures procedures)
        {
            InitializeComponent();
            registr.Height = 0;
            registr.Width = 0;
            procedurenew = procedures;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            int counter = 0;
            int indexspez = 0;
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
                            counter++;
                            Classes.Spezialization spezialization = new Classes.Spezialization();

                            spezialization.Spezializationname = reader.GetValue(0);

                            comdobox = (string)spezialization.Spezializationname;
                            Spezialization.Items.Add(comdobox);
                            if ((string)procedurenew.Spezialization == comdobox)
                            {
                                indexspez = counter;
                            }
                        }
                    }
                    Spezialization.SelectedIndex = indexspez;
                    Name.Text = (string)procedurenew.Name_Procedures;
                    Price.Text = Convert.ToString(procedurenew.Price);
                    Decription.Text = (string)procedurenew.Decription;
                    imageBytes = procedurenew.Photo_Procedures_byte;
                    if (procedurenew.Photo_Procedures_byte != null)
                    {

                        var image = new BitmapImage();


                        using (var mem = new MemoryStream((procedurenew.Photo_Procedures_byte)))
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
                        Photo.Source = image;

                    }
                }
                catch
                {

                }
            }
        }
        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "название")
            {
                Name.Text = "";
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
                Name.Text = "название";
            name = true;
            Warring.Text = "";
            if ((Name.Text.ToString().Contains(allowedchar)))
            {
                name = false;
                Warring.Text = "Текст содержит запрещенные знаки";
                return;
            }
            string sql = $"SELECT * FROM Procedures";


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                if (Name.Text != "название")
                {
                    Regex regex = new Regex(@"[0-9]+");
                    if (regex.IsMatch(Name.Text))
                    {
                        Warring.Text = "Некорректно задано название";
                        names = false;
                    }
                    else if (!regex.IsMatch(Name.Text))
                    {
                        names = true;
                        Warring.Text = "";
                        if ((string)procedurenew.Name_Procedures != Name.Text)
                        {
                            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                            SqlDataReader reader = sqlCommand.ExecuteReader();

                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    if (Name.Text == (string)reader.GetValue(1))
                                    {
                                        string imagePath = System.IO.Path.Combine(_basePath, "image", "no.png");
                                        Yesornologine.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                                        names = false;
                                        break;
                                    }
                                    else
                                    {
                                        names = true;
                                        string imagePath = System.IO.Path.Combine(_basePath, "image", "yes.png");
                                        Yesornologine.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                                    }
                                }
                                reader.Close();

                            }
                        }
                        else
                        {
                            names = true;
                        }
                    }
                }

            }
        }

        private void Price_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Price.Text == "цена")
            {
                Price.Text = "";
            }
        }

        private void Price_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Price.Text))
                Price.Text = "цена";
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(Price.Text))
            {
                Warring.Text = "Некорректно задана цена";
                price = false;
            }
            else if (regex.IsMatch(Price.Text))
            {
                price = true;
                Warring.Text = "";
            }

        }

        private void TakePhoto_Click(object sender, RoutedEventArgs e)
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
                Photo.Source = new BitmapImage(new Uri(openFile.FileName));
            }

        }

        private void Decription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Decription.Text == "краткое описание")
            {
                Decription.Text = "";
            }
        }

        private void Decription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Decription.Text))
                Decription.Text = "краткое описание";
            if (Decription.Text != "краткое описание")
            {
                if (Decription.Text.Length > 100)
                {
                    decrp = false;
                    Warring.Text = "Сократите описание";

                }
                else
                {
                    decrp = true;
                    Warring.Text = "";
                }
            }
            if ((Decription.Text.ToString().Contains(allowedchar)))
            {
                decrp = false;
                Warring.Text = "Текст содержит запрещенные знаки";
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "название")
            {
                Warring.Text = "Вы не ввели название";
                return;
            }
            if (!names)
            {
                Warring.Text = "Некорректное поле с названием";
                return;
            }
            if (!decrp)
            {
                Warring.Text = "Сократите описание";
                return;
            }
            if (Price.Text == "цена")
            {
                Warring.Text = "Вы не ввели цену";
                return;
            }
            if (Decription.Text == "краткое описание")
            {
                Warring.Text = "Вы не ввели описание";
                return;
            }
            if (Spezialization.Text == "Выберите")
            {
                Warring.Text = "Вы не выбрали специальность";
                return;
            }
            if ((Name.Text != "название") && (Price.Text != "цена") && (Decription.Text != "краткое описание") && (Spezialization.Text != "Выберите") && (names) && (decrp) && name && price)
            {
                if (imageBytes == null)
                {
                    byte[] imageBytesBuffer;
                    string imagePath = System.IO.Path.Combine(_basePath, "image", "injection.png");

                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        imageBytesBuffer = new byte[fs.Length];
                        fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                    }
                    imageBytes = imageBytesBuffer;
                }



                string sql = $"INSERT INTO Procedures ([Name_Procedures],[Price],[Decription],[Spezialization],[Photo]) VALUES " +
                    $"('{Name.Text}','{Price.Text}','{Decription.Text}','{Spezialization.Text}',@bytes)";
                try
                {

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command1 = new SqlCommand(sql, sqlConnection);
                        command1.Parameters.AddWithValue("@bytes", imageBytes);
                        command1.ExecuteNonQuery();
                        MessangeBox_Ok messange = new MessangeBox_Ok("Процедура зарегистрирована!");
                        messange.Show();
                        this.Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (!names || !decrp || !name || !price)
            {
                Warring.Text = "Проверьте данные на корректность";
            }

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "название")
            {
                Warring.Text = "Вы не ввели название";
                return;
            }
            if (!names)
            {
                Warring.Text = "Некорректное поле с названием";
                return;
            }
            if (!decrp)
            {
                Warring.Text = "Сократите описание";
                return;
            }
            if (Price.Text == "цена")
            {
                Warring.Text = "Вы не ввели цену";
                return;
            }
            if (Decription.Text == "краткое описание")
            {
                Warring.Text = "Вы не ввели описание";
                return;
            }
            if (Spezialization.Text == "Выберите")
            {
                Warring.Text = "Вы не выбрали специальность";
                return;
            }
            if ((Name.Text != "название") && (Price.Text != "цена") && (Decription.Text != "краткое описание") && (Spezialization.Text != "Выберите") && (names) && (decrp) && name && price)
            {
                if (imageBytes == null)
                {
                    byte[] imageBytesBuffer;
                    string imagePath = System.IO.Path.Combine(_basePath, "image", "injection.png");

                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        imageBytesBuffer = new byte[fs.Length];
                        fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                    }
                    imageBytes = imageBytesBuffer;
                }
                int id = (int)procedurenew.ID_Procedures;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression1 = $"UPDATE [Procedures] SET Name_Procedures = '{Name.Text}', Decription = '{Decription.Text}', Price = '{Price.Text}', Photo = @bytes, Spezialization = '{Spezialization.Text}' WHERE ID_Procedures = '{id}'";
                        SqlCommand command = new SqlCommand(sqlExpression1, connection);
                        command.Parameters.AddWithValue("@bytes", imageBytes);
                        command.ExecuteNonQuery();
                        Deleteprosedurec deleteprosedurec = new Deleteprosedurec();
                        deleteprosedurec.Show();
                        this.Close();
                    }
                    catch
                    {

                    }

                }
            }
            else if (!names || !decrp || !name || !price)
            {
                Warring.Text = "Проверьте данные на корректность";
            }


        }


        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Deleteprosedurec deleteprosedurec = new Deleteprosedurec();
            deleteprosedurec.Show();
            this.Close();
        }
    }
}

using MedicalCenter.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Логика взаимодействия для RegistrationofDoctor.xaml
    /// </summary>
    public partial class RegistrationofDoctor : Window
    {
        DateTime date;
        int dayofweek;
        public byte[] imageBytes;
        string connectionString;
        bool names = true;
        bool workyears = true;
        bool study = true;
        bool surname = true;
        bool lastname = true;
        bool firstname = true;
        string allowedchar = "'";

        bool loginValid = false;
        bool passwordValid = false;

        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory;
        public RegistrationofDoctor()
        {

            InitializeComponent();
            Mondfrom.SelectedIndex = 0;
            Mondto.SelectedIndex = 0;
            Tuefrom.SelectedIndex = 0;
            Tueto.SelectedIndex = 0;
            Wenfrom.SelectedIndex = 0;
            Wento.SelectedIndex = 0;
            Thufrom.SelectedIndex = 0;
            Thuto.SelectedIndex = 0;
            Frifrom.SelectedIndex = 0;
            Frito.SelectedIndex = 0;
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
                            Specialization.Items.Add(comdobox);

                        }
                    }
                    Specialization.SelectedIndex = 0;
                }
                catch
                {

                }
            }
        }
        private void name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (name.Text == "имя")
            {
                name.Text = "";
            }
        }

        private void name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text))
                name.Text = "имя";
            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(name.Text))
            {
                Warring.Text = "Некорректно задано имя";
                lastname = false;
            }
            else if (regex.IsMatch(name.Text))
            {
                Warring.Text = "";
                lastname = true;
            }
            if (name.Text != "имя" && Last_name.Text != "отчество" && Surrename.Text != "фамилия")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression3 = $"SELECT * FROM Doctor ";
                        SqlCommand sqlCommand2 = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader2 = sqlCommand2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {

                                if (name.Text == (string)reader2.GetValue(1) && Last_name.Text == (string)reader2.GetValue(3) && Surrename.Text == (string)reader2.GetValue(2))
                                {
                                    Warring.Text = "Этот врач уже существует";
                                    names = false;
                                    break;
                                }
                                else
                                {
                                    names = true;
                                }

                            }

                        }

                        reader2.Close();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Last_name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Last_name.Text == "отчество")
            {
                Last_name.Text = "";
            }
        }

        private void Last_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Last_name.Text))
                Last_name.Text = "отчество";
            Regex regex = new Regex(@"^[A-ЯЁ][а-яё]+$");
            if (!regex.IsMatch(Last_name.Text))
            {
                Warring.Text = "Некорректно задано отчество";
                lastname = false;
            }
            else if (regex.IsMatch(Last_name.Text))
            {
                Warring.Text = "";
                lastname = true;
            }
            if (name.Text != "имя" && Last_name.Text != "отчество" && Surrename.Text != "фамилия")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression3 = $"SELECT * FROM Doctor ";
                        SqlCommand sqlCommand2 = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader2 = sqlCommand2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {

                                if (name.Text == (string)reader2.GetValue(1) && Last_name.Text == (string)reader2.GetValue(3) && Surrename.Text == (string)reader2.GetValue(2))
                                {
                                    Warring.Text = "Этот врач уже существует";
                                    names = false;
                                    break;
                                }
                                else
                                {
                                    names = true;
                                }

                            }

                        }

                        reader2.Close();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
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
                Warring.Text = "Некорректно задана фамилия";
                surname = false;
            }
            else if (regex.IsMatch(Surrename.Text))
            {
                Warring.Text = "";
                surname = true;
            }
            if (name.Text != "имя" && Last_name.Text != "отчество" && Surrename.Text != "фамилия")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression3 = $"SELECT * FROM Doctor ";
                        SqlCommand sqlCommand2 = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader2 = sqlCommand2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {

                                if (name.Text == (string)reader2.GetValue(1) && Last_name.Text == (string)reader2.GetValue(3) && Surrename.Text == (string)reader2.GetValue(2))
                                {
                                    Warring.Text = "Этот врач уже существует";
                                    names = false;
                                    break;
                                }
                                else
                                {
                                    names = true;
                                }

                            }

                        }

                        reader2.Close();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Study_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Study.Text == "ученая степень")
            {
                Study.Text = "";
            }
        }

        private void Study_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Study.Text))
                Study.Text = "ученая степень";
            string str = Study.Text;
            Regex regex = new Regex(@"[0-9]+");
            if (regex.IsMatch(str))
            {
                Warring.Text = "Некорректно задана ученая степень";
                study = false;
            }
            else if (!regex.IsMatch(str))
            {
                Warring.Text = "";
                study = true;
            }
            if ((Study.Text.ToString().Contains(allowedchar)))
            {
                study = false;
                Warring.Text = "Текст содержит заперещенные знаки";
                return;
            }
        }

        private void Work_GotFocus(object sender, RoutedEventArgs e)
        {

            if (Work.Text == "начало карьеры")
            {
                Work.Text = "";
                return;
            }


        }

        private void Work_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Work.Text))
                Work.Text = "начало карьеры";
            string str = Work.Text;
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(str))
            {
                Warring.Text = "Некорректно задан год начала карьеры";
                workyears = false;
            }
            else if (regex.IsMatch(str))
            {
                if ((Convert.ToInt32(Work.Text) < 1955) || (Convert.ToInt32(Work.Text) > 2022))
                {
                    Warring.Text = "Некорректно задан год начала карьеры";
                    workyears = false;
                }
                else
                {
                    Warring.Text = "";
                    workyears = true;
                }
            }
        }
        private void DoctorLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DoctorLogin.Text == "логин")
            {
                DoctorLogin.Text = "";
                DoctorLogin.Foreground = Brushes.Black;
            }
        }

        private void DoctorLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DoctorLogin.Text))
            {
                DoctorLogin.Text = "логин";
                DoctorLogin.Foreground = Brushes.Gray;
            }
        }

        private void DoctorPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DoctorPassword.Text == "пароль")
            {
                DoctorPassword.Text = "";
                DoctorPassword.Foreground = Brushes.Black;
            }
        }

        private void DoctorPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DoctorPassword.Text))
            {
                DoctorPassword.Text = "пароль";
                DoctorPassword.Foreground = Brushes.Gray;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((Mondfrom.Text != Mondto.Text) && (Tuefrom.Text != Tueto.Text) && (Wenfrom.Text != Wento.Text) && (Thufrom.Text != Thuto.Text) && (Frifrom.Text != Frito.Text) && (Mondfrom.Text != "Выберите") && (Mondto.Text != "Выберите") && (Tuefrom.Text != "Выберите") && (Tueto.Text != "Выберите") && (Wenfrom.Text != "Выберите") && (Wento.Text != "Выберите") && (Thufrom.Text != "Выберите") && (Thuto.Text != "Выберите") && (Frifrom.Text != "Выберите") && (Frito.Text != "Выберите") && names && workyears && study && lastname && surname && firstname)
            {

                if ((Last_name.Text != "отчество") && (Work.Text != "начало карьеры") && (name.Text != "имя") && (Surrename.Text != "фамилия") && (Study.Text != "ученая степень") && (Specialization.Text != "Выберите"))
                {
                    if (imageBytes == null)
                    {
                        string imagePath = System.IO.Path.Combine(_basePath, "image", "doctor.png");
                        byte[] imageBytesBuffer;

                        using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                        {
                            imageBytesBuffer = new byte[fs.Length];
                            fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                        }
                        imageBytes = imageBytesBuffer;
                    }

                    // Проверка уникальности логина
                    string checkLoginQuery = "SELECT COUNT(*) FROM Doctor WHERE DoctorLogin = @Login";
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand checkCmd = new SqlCommand(checkLoginQuery, connection);
                            checkCmd.Parameters.AddWithValue("@Login", DoctorLogin.Text);

                            // Безопасное преобразование результата
                            object result = checkCmd.ExecuteScalar();
                            int count = (result == DBNull.Value || result == null) ? 0 : Convert.ToInt32(result);

                            if (count > 0)
                            {
                                Warring.Text = "Логин уже занят!";
                                return;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Ошибка базы данных: {ex.Message}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                        return;
                    }


                    string sql = $"INSERT INTO Doctor ([Name_Doctor],[Surname_Doctor],[Middle_name_Doctor],[DoctorLogin],[DoctorPassword],[Spezialization],[Study],[Work_experience],[Photo_Doctor]) VALUES " +
                         $"('{name.Text}','{Surrename.Text}','{Last_name.Text}','{DoctorLogin.Text}','{DoctorPassword.Text}','{Specialization.Text}','{Study.Text}','{Work.Text}',@bytes)";

                    string sql2 = "SELECT TOP 1 * FROM Doctor ORDER BY ID_Doctor DESC";


                    try
                    {

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            SqlCommand command1 = new SqlCommand(sql, sqlConnection);
                            command1.Parameters.AddWithValue("@bytes", imageBytes);
                            command1.ExecuteNonQuery();
                            SqlCommand command2 = new SqlCommand(sql2, sqlConnection);
                            SqlDataReader reader = command2.ExecuteReader();
                            Classes.Doctor doctor = new Classes.Doctor();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    doctor.ID_Doctor = reader.GetValue(0);
                                    doctor.Name_Doctor = reader.GetValue(1);
                                    doctor.Surname_Doctor = reader.GetValue(2);
                                    doctor.Middle_name_Doctor = reader.GetValue(3);
                                    doctor.Spezialization = reader.GetValue(4);
                                    doctor.Study = reader.GetValue(5);
                                    doctor.Work_experience = reader.GetValue(6);
                                    doctor.Photo_Doctor = reader.GetValue(7);
                                    ////
                                    doctor.DoctorLogin = reader.GetValue(8);
                                    doctor.DoctorPassword = reader.GetValue(9);
                                }
                            }
                            reader.Close();
                            string sql3 = $"INSERT INTO schedule ([ID_doctor],[Mondfrom],[Mondto],[Tuefrom],[Tueto],[Wenfrom],[Wento],[Thufrom],[Thuto],[Frifrom],[Frito]) VALUES " +
                               $"('{doctor.ID_Doctor}','{Mondfrom.Text}','{Mondto.Text}','{Tuefrom.Text}','{Tueto.Text}','{Wenfrom.Text}','{Wento.Text}','{Thufrom.Text}','{Thuto.Text}','{Frifrom.Text}','{Frito.Text}')";
                            SqlCommand command3 = new SqlCommand(sql3, sqlConnection);
                            command3.ExecuteNonQuery();
                            MessangeBox_Ok messange = new MessangeBox_Ok("Доктор зарегистрирован!");
                            messange.Show();
                            date = DateTime.Now;
                            DateTime dateplusday = date.AddDays(1);
                            DateTime dateplusmounth = date.AddMonths(1);

                            while (dateplusday < dateplusmounth)
                            {
                                dayofweek = (int)dateplusday.DayOfWeek;
                                switch (dayofweek)
                                {
                                    case 1: // Понедельник
                                        {
                                            DateTime fromTime;
                                            DateTime toTime;

                                            if (!DateTime.TryParseExact(Mondfrom.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromTime) ||
                                                !DateTime.TryParseExact(Mondto.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toTime))
                                            {
                                                Warring.Text = "Некорректный формат времени для понедельника";
                                                return;
                                            }

                                            while (fromTime < toTime)
                                            {
                                                using (SqlCommand cmd = new SqlCommand(
                                                    "INSERT INTO Coupon ([Date], [time], [ID_Doctor], [Ordered]) VALUES (@Date, @Time, @DoctorID, 'no')",
                                                    sqlConnection))
                                                {
                                                    cmd.Parameters.AddWithValue("@Date", dateplusday.Date);
                                                    cmd.Parameters.AddWithValue("@Time", fromTime.ToString("HH:mm"));
                                                    cmd.Parameters.AddWithValue("@DoctorID", doctor.ID_Doctor);
                                                    cmd.ExecuteNonQuery();
                                                }
                                                fromTime = fromTime.AddMinutes(30);
                                            }
                                            dateplusday = dateplusday.AddDays(1);
                                            break;
                                        }

                                    case 2: // Вторник
                                        {
                                            DateTime fromTime;
                                            DateTime toTime;

                                            if (!DateTime.TryParseExact(Tuefrom.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromTime) ||
                                                !DateTime.TryParseExact(Tueto.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toTime))
                                            {
                                                Warring.Text = "Некорректный формат времени для вторника";
                                                return;
                                            }

                                            while (fromTime < toTime)
                                            {
                                                using (SqlCommand cmd = new SqlCommand(
                                                    "INSERT INTO Coupon ([Date], [time], [ID_Doctor], [Ordered]) VALUES (@Date, @Time, @DoctorID, 'no')",
                                                    sqlConnection))
                                                {
                                                    cmd.Parameters.AddWithValue("@Date", dateplusday.Date);
                                                    cmd.Parameters.AddWithValue("@Time", fromTime.ToString("HH:mm"));
                                                    cmd.Parameters.AddWithValue("@DoctorID", doctor.ID_Doctor);
                                                    cmd.ExecuteNonQuery();
                                                }
                                                fromTime = fromTime.AddMinutes(30);
                                            }
                                            dateplusday = dateplusday.AddDays(1);
                                            break;
                                        }

                                    case 3: // Среда
                                        {
                                            DateTime fromTime;
                                            DateTime toTime;

                                            if (!DateTime.TryParseExact(Wenfrom.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromTime) ||
                                                !DateTime.TryParseExact(Wento.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toTime))
                                            {
                                                Warring.Text = "Некорректный формат времени для среды";
                                                return;
                                            }

                                            while (fromTime < toTime)
                                            {
                                                using (SqlCommand cmd = new SqlCommand(
                                                    "INSERT INTO Coupon ([Date], [time], [ID_Doctor], [Ordered]) VALUES (@Date, @Time, @DoctorID, 'no')",
                                                    sqlConnection))
                                                {
                                                    cmd.Parameters.AddWithValue("@Date", dateplusday.Date);
                                                    cmd.Parameters.AddWithValue("@Time", fromTime.ToString("HH:mm"));
                                                    cmd.Parameters.AddWithValue("@DoctorID", doctor.ID_Doctor);
                                                    cmd.ExecuteNonQuery();
                                                }
                                                fromTime = fromTime.AddMinutes(30);
                                            }
                                            dateplusday = dateplusday.AddDays(1);
                                            break;
                                        }

                                    case 4: // Четверг
                                        {
                                            DateTime fromTime;
                                            DateTime toTime;

                                            if (!DateTime.TryParseExact(Thufrom.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromTime) ||
                                                !DateTime.TryParseExact(Thuto.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toTime))
                                            {
                                                Warring.Text = "Некорректный формат времени для четверга";
                                                return;
                                            }

                                            while (fromTime < toTime)
                                            {
                                                using (SqlCommand cmd = new SqlCommand(
                                                    "INSERT INTO Coupon ([Date], [time], [ID_Doctor], [Ordered]) VALUES (@Date, @Time, @DoctorID, 'no')",
                                                    sqlConnection))
                                                {
                                                    cmd.Parameters.AddWithValue("@Date", dateplusday.Date);
                                                    cmd.Parameters.AddWithValue("@Time", fromTime.ToString("HH:mm"));
                                                    cmd.Parameters.AddWithValue("@DoctorID", doctor.ID_Doctor);
                                                    cmd.ExecuteNonQuery();
                                                }
                                                fromTime = fromTime.AddMinutes(30);
                                            }
                                            dateplusday = dateplusday.AddDays(1);
                                            break;
                                        }

                                    case 5: // Пятница
                                        {
                                            DateTime fromTime;
                                            DateTime toTime;

                                            if (!DateTime.TryParseExact(Frifrom.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromTime) ||
                                                !DateTime.TryParseExact(Frito.Text, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toTime))
                                            {
                                                Warring.Text = "Некорректный формат времени для пятницы";
                                                return;
                                            }

                                            while (fromTime < toTime)
                                            {
                                                using (SqlCommand cmd = new SqlCommand(
                                                    "INSERT INTO Coupon ([Date], [time], [ID_Doctor], [Ordered]) VALUES (@Date, @Time, @DoctorID, 'no')",
                                                    sqlConnection))
                                                {
                                                    cmd.Parameters.AddWithValue("@Date", dateplusday.Date);
                                                    cmd.Parameters.AddWithValue("@Time", fromTime.ToString("HH:mm"));
                                                    cmd.Parameters.AddWithValue("@DoctorID", doctor.ID_Doctor);
                                                    cmd.ExecuteNonQuery();
                                                }
                                                fromTime = fromTime.AddMinutes(30);
                                            }
                                            dateplusday = dateplusday.AddDays(1);
                                            break;
                                        }
                                    case 6:
                                        dateplusday = dateplusday.AddDays(1);
                                        break;
                                    case 0:
                                        dateplusday = dateplusday.AddDays(1);
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    this.Close();

                }
            }
            if ((Mondfrom.Text == Mondto.Text) || (Tuefrom.Text == Tueto.Text) || (Wenfrom.Text == Wento.Text) || (Thufrom.Text == Thuto.Text) || (Frifrom.Text == Frito.Text) || (Mondfrom.Text == "Выберите") || (Mondto.Text == "Выберите") || (Tuefrom.Text == "Выберите") || (Tueto.Text == "Выберите") || (Wenfrom.Text == "Выберите") || (Wento.Text == "Выберите") || (Thufrom.Text == "Выберите") || (Thuto.Text == "Выберите") || (Frifrom.Text == "Выберите") || (Frito.Text == "Выберите"))
            {
                Warring.Text = "Некорректно задан график работы";
                return;
            }

            if (name.Text == "имя")
            {
                Warring.Text = "Не задано имя";
                return;
            }
            if (Surrename.Text == "фамилия")
            {
                Warring.Text = "Не задана фамилия";
                return;
            }
            if (Last_name.Text == "отчество")
            {
                Warring.Text = "Не задано отчество";
                return;
            }
            if (Specialization.Text == "Выберите")
            {
                Warring.Text = "Не задана специальность";
                return;
            }
            if (Study.Text == "ученая степень")
            {
                Warring.Text = "Не задана ученая степень";
                return;
            }
            if (Work.Text == "начало карьеры")
            {

                Warring.Text = "Не задан год начала карьеры";
                return;

            }
            string str = Work.Text;
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(str))
            {
                if ((Convert.ToInt32(Work.Text) < 1955) || (Convert.ToInt32(Work.Text) > 2022))
                    Warring.Text = "Некорректно задан год начала карьеры";
            }
            else
            {
                Warring.Text = "Некорректно задан год начала карьеры";
            }
            if (!names || !workyears || !study || !lastname || !surname || !firstname)
            {
                Warring.Text = "Перепроверьте точность введенных данных";
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
    }
}

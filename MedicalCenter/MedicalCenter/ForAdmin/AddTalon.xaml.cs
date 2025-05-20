using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using MedicalCenter.Windows;

namespace MedicalCenter.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для AddTalon.xaml
    /// </summary>
    public partial class AddTalon : Window
    {
        int dayofweek;
        string connectionString;
        public List<Classes.Doctor> doctors = new List<Classes.Doctor>();
        Classes.schedule schedule = new Classes.schedule();
        int ID_D;
        public AddTalon()
        {
            InitializeComponent();
            Doctorname.SelectedIndex = 0;
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Doctorname_DropDownClosed(object sender, EventArgs e)
        {
            Dateone.Items.Clear();
            Datetwo.Items.Clear();

            if (Doctorname.Text == "Врач")
            {
                return;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        foreach (var doc in doctors)
                        {
                            string fullName = $"{doc.Surname_Doctor} {doc.Name_Doctor} {doc.Middle_name_Doctor}";
                            if (fullName == Doctorname.Text)
                            {
                                ID_D = (int)doc.ID_Doctor;
                                MessageBox.Show($"Выбран врач: {fullName}, ID: {ID_D}");
                                break;
                            }
                        }

                        // Добавляем даты на 2 недели вперед в Dateone
                        DateTime currentDate = DateTime.Today;
                        for (int i = 0; i < 14; i++)
                        {
                            DateTime dateToAdd = currentDate.AddDays(i);
                            // Добавляем только рабочие дни (пн-пт)
                            if (dateToAdd.DayOfWeek != DayOfWeek.Saturday && dateToAdd.DayOfWeek != DayOfWeek.Sunday)
                            {
                                Dateone.Items.Add(dateToAdd.ToString("dd.MM.yyyy")); // Используем явный формат даты
                            }
                        }

                        // Остальной код остается без изменений
                        string sqlExpression3 = $"Select * from schedule where ID_doctor={ID_D}";
                        SqlCommand sqlCommand2 = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader1 = sqlCommand2.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                Mondfrom.Items.Add(reader1.GetValue(1));
                                Mondto.Items.Add(reader1.GetValue(2));
                                Tuefrom.Items.Add(reader1.GetValue(3));
                                Tueto.Items.Add(reader1.GetValue(4));
                                Wenfrom.Items.Add(reader1.GetValue(5));
                                Wento.Items.Add(reader1.GetValue(6));
                                Thufrom.Items.Add(reader1.GetValue(7));
                                Thuto.Items.Add(reader1.GetValue(8));
                                Frifrom.Items.Add(reader1.GetValue(9));
                                Frito.Items.Add(reader1.GetValue(10));
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
                            }
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Dateone_DropDownClosed_1(object sender, EventArgs e)
        {
            Datetwo.Items.Clear();

            if (!string.IsNullOrEmpty(Dateone.Text))
            {
                try
                {
                    DateTime selectedDate = DateTime.ParseExact(Dateone.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                    // Добавляем даты на 2 недели вперед от выбранной даты
                    for (int i = 1; i <= 14; i++)
                    {
                        DateTime dateToAdd = selectedDate.AddDays(i);
                        // Добавляем только рабочие дни (пн-пт)
                        if (dateToAdd.DayOfWeek != DayOfWeek.Saturday && dateToAdd.DayOfWeek != DayOfWeek.Sunday)
                        {
                            Datetwo.Items.Add(dateToAdd.ToString("dd.MM.yyyy")); // Используем тот же формат
                        }
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Некорректный формат даты. Пожалуйста, выберите дату из списка.");
                }
            }
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime startDate = DateTime.ParseExact(Dateone.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(Datetwo.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                    {
                        // Пропускаем выходные
                        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                            continue;

                        // Получаем время начала и окончания для текущего дня
                        TimeSpan fromTime = GetTimeFromComboBox(currentDate.DayOfWeek, isFromTime: true);
                        TimeSpan toTime = GetTimeFromComboBox(currentDate.DayOfWeek, isFromTime: false);

                        // Генерация талонов с шагом 30 минут
                        for (TimeSpan time = fromTime; time < toTime; time = time.Add(TimeSpan.FromMinutes(30)))
                        {
                            // Проверка на существующий талон
                            string checkSql = $"SELECT COUNT(*) FROM Coupon WHERE Date='{currentDate:yyyy-MM-dd}' " +
                                             $"AND time='{time:hh\\:mm\\:ss}' AND ID_Doctor={ID_D}";

                            SqlCommand checkCmd = new SqlCommand(checkSql, connection);
                            int exists = (int)checkCmd.ExecuteScalar();

                            if (exists == 0)
                            {
                                string insertSql = $"INSERT INTO Coupon (Date, time, ID_Doctor, Ordered) " +
                                                 $"VALUES ('{currentDate:yyyy-MM-dd}', '{time:hh\\:mm\\:ss}', {ID_D}, 'no')";
                                new SqlCommand(insertSql, connection).ExecuteNonQuery();
                            }
                        }
                    }

                    // Используем MessangeBox_Ok для отображения сообщения
                    MessangeBox_Ok messange = new MessangeBox_Ok("Талоны успешно добавлены!");
                    messange.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private TimeSpan GetTimeFromComboBox(DayOfWeek dayOfWeek, bool isFromTime)
        {
            try
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return DateTime.Parse(isFromTime ? Mondfrom.Text : Mondto.Text).TimeOfDay;
                    case DayOfWeek.Tuesday:
                        return DateTime.Parse(isFromTime ? Tuefrom.Text : Tueto.Text).TimeOfDay;
                    case DayOfWeek.Wednesday:
                        return DateTime.Parse(isFromTime ? Wenfrom.Text : Wento.Text).TimeOfDay;
                    case DayOfWeek.Thursday:
                        return DateTime.Parse(isFromTime ? Thufrom.Text : Thuto.Text).TimeOfDay;
                    case DayOfWeek.Friday:
                        return DateTime.Parse(isFromTime ? Frifrom.Text : Frito.Text).TimeOfDay;
                    default:
                        throw new ArgumentException("Не рабочий день");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка получения времени: {ex.Message}");
            }
        }
    }
}

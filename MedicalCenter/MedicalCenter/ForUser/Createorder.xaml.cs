using MedicalCenter.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Configuration; // Для ConfigurationManager

namespace MedicalCenter.ForUser
{
    /// <summary>
    /// Логика взаимодействия для Createorder.xaml
    /// </summary>
    public partial class Createorder : Window
    {
        string connectionString;
        List<Classes.Doctor> doctors = new List<Classes.Doctor>();
        List<Classes.Procedures> procedures = new List<Classes.Procedures>();
        List<Classes.Coupon> coupons = new List<Classes.Coupon>();
        Classes.Clients client = new Classes.Clients();
        public Createorder(Classes.Clients clients)
        {
            InitializeComponent();
            this.client = clients;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT * FROM Procedures";
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
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
                            Spesname.Items.Add(procedure.Name_Procedures);
                        }
                    }
                    Spesname.SelectedIndex = 0;
                    reader.Close();
                    string sqlExpression3 = $"SELECT * FROM Doctor ";
                    string comdobox;
                    SqlCommand sqlCommand2 = new SqlCommand(sqlExpression3, connection);
                    SqlDataReader reader2 = sqlCommand2.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            Classes.Doctor doctor = new Classes.Doctor();
                            doctor.ID_Doctor = reader2.GetValue(0);
                            doctor.Name_Doctor = reader2.GetValue(1);
                            doctor.Surname_Doctor = reader2.GetValue(2);
                            doctor.Middle_name_Doctor = reader2.GetValue(3);
                            doctor.Photo_Doctor = reader2.GetValue(7);
                            doctor.Photo_Doctor_byte = (byte[])reader2.GetValue(7);
                            comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                            DoctorName.Items.Add(comdobox);
                            doctors.Add(doctor);
                        }
                    }
                    DoctorName.SelectedIndex = 0;
                    reader2.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Spesname_DropDownClosed(object sender, EventArgs e)
        {
            if (Spesname.Text == "Процедура" && DoctorName.Text != "Врач")
            {
                string text = DoctorName.Text;
                DoctorName.Items.Clear();
                DoctorName.Items.Add("Врач");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
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
                            doctor.Photo_Doctor = reader.GetValue(7);
                            doctor.Photo_Doctor_byte = (byte[])reader.GetValue(7);
                            comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;

                            DoctorName.Items.Add(comdobox);
                            doctors.Add(doctor);
                        }
                    }

                    for (int i = 0; i < DoctorName.Items.Count; i++)
                    {
                        DoctorName.SelectedIndex = i;
                        if (DoctorName.Text == text)
                        { break; }
                    }

                    connection.Close();

                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    procedures.Clear();
                    Spesname.Items.Clear();
                    Spesname.Items.Add("Процедура");
                    Spesname.SelectedIndex = 0;
                    connection.Open();

                    string sqlExpression2 = null;
                    foreach (var doc in doctors)
                    {
                        if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                        {
                            sqlExpression2 = $"SELECT [Procedures].ID_Procedures,[Procedures].Name_Procedures,[Procedures].Decription,[Procedures].Price,[Procedures].Photo,[Procedures].Spezialization FROM [Procedures] " +
                            $"inner join Doctor on [Procedures].Spezialization=Doctor.Spezialization and Doctor.ID_Doctor={doc.ID_Doctor}";
                            break;
                        }

                    }
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
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
                            Spesname.Items.Add(procedure.Name_Procedures);
                        }
                    }
                    reader.Close();

                }
            }

            if (Spesname.Text != "Процедура" && DoctorName.Text == "Врач")
            {
                Date.Items.Clear();
                Time.Items.Clear();
                foto.Source = null;
                DoctorName.Items.Clear();
                DoctorName.Items.Add("Врач");
                DoctorName.SelectedIndex = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = $"SELECT Doctor.ID_Doctor,Doctor.Name_Doctor,Doctor.Surname_Doctor,Doctor.Middle_name_Doctor,Doctor.Photo_Doctor,Doctor.Photo_Doctor FROM Doctor inner join [Procedures] on Doctor.Spezialization=[Procedures].Spezialization  and [Procedures].Name_Procedures='{Spesname.Text}'";
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
                                doctor.Photo_Doctor = reader.GetValue(4);
                                doctor.Photo_Doctor_byte = (byte[])reader.GetValue(4);
                                comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                                DoctorName.Items.Add(comdobox);
                                doctors.Add(doctor);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            if (Spesname.Text == "Процедура" && DoctorName.Text == "Врач")
            {
                Date.Items.Clear();
                Time.Items.Clear();
                foto.Source = null;
                DoctorName.Items.Clear();
                DoctorName.Items.Add("Врач");
                DoctorName.SelectedIndex = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = $"SELECT* FROM Doctor";
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
                                doctor.Photo_Doctor = reader.GetValue(7);
                                doctor.Photo_Doctor_byte = (byte[])reader.GetValue(7);
                                comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                                DoctorName.Items.Add(comdobox);
                                doctors.Add(doctor);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                Spesname.Items.Clear();
                Spesname.Items.Add("Процедура");
                Spesname.SelectedIndex = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = $"SELECT * FROM Procedures";
                        SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
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
                                Spesname.Items.Add(procedure.Name_Procedures);
                            }
                        }
                        Spesname.SelectedIndex = 0;
                        reader.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void DoctorName_DropDownClosed(object sender, EventArgs e)
        {
            if (Spesname.Text == "Процедура" && DoctorName.Text != "Врач")
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Spesname.Items.Clear();
                    Spesname.Items.Add("Процедура");
                    Spesname.SelectedIndex = 0;
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = null;
                        foreach (var doc in doctors)
                        {
                            if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                            {
                                sqlExpression2 = $"SELECT [Procedures].ID_Procedures,[Procedures].Name_Procedures,[Procedures].Decription,[Procedures].Price,[Procedures].Photo,[Procedures].Spezialization FROM [Procedures] " +
                                $"inner join Doctor on [Procedures].Spezialization=Doctor.Spezialization and Doctor.ID_Doctor={doc.ID_Doctor}";
                                var image = new BitmapImage();


                                using (var mem = new MemoryStream((doc.Photo_Doctor_byte)))
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
                                foto.Source = image;

                            }


                        }
                        SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
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
                                Spesname.Items.Add(procedure.Name_Procedures);
                            }
                        }
                        reader.Close();
                        Date.Items.Clear();
                        string sqlExpression3 = $"SELECT DISTINCT Date  from Coupon where Ordered='no' and Date>GETDATE()";
                        SqlCommand sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                        SqlDataReader reader1 = sqlCommand1.ExecuteReader();
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                Classes.Coupon coupon = new Classes.Coupon();

                                coupon.Date = reader1.GetValue(0);

                                coupons.Add(coupon);
                                Date.Items.Add(coupon.Date);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            if (Spesname.Text != "Процедура" && DoctorName.Text != "Врач")
            {
                string text = Spesname.Text;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Spesname.Items.Clear();
                    Spesname.Items.Add("Процедура");
                    Spesname.SelectedIndex = 0;
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = null;
                        foreach (var doc in doctors)
                        {
                            if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                            {
                                sqlExpression2 = $"SELECT [Procedures].ID_Procedures,[Procedures].Name_Procedures,[Procedures].Decription,[Procedures].Price,[Procedures].Photo,[Procedures].Spezialization FROM [Procedures] " +
                                $"inner join Doctor on [Procedures].Spezialization=Doctor.Spezialization and Doctor.ID_Doctor={doc.ID_Doctor}";
                                var image = new BitmapImage();


                                using (var mem = new MemoryStream((doc.Photo_Doctor_byte)))
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
                                foto.Source = image;
                                break;
                            }


                        }


                        SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
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
                                Spesname.Items.Add(procedure.Name_Procedures);
                            }
                        }
                        reader.Close();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    for (int i = 0; i < Spesname.Items.Count; i++)
                    {
                        Spesname.SelectedIndex = i;
                        if (Spesname.Text == text)
                        { break; }
                    }
                    Date.Items.Clear();
                    string sqlExpression3 = null;
                    foreach (var doc in doctors)
                    {
                        if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                        {
                            sqlExpression3 = $"SELECT DISTINCT Date  from Coupon where Ordered='no' and ID_Doctor='{doc.ID_Doctor}' and Date>GETDATE()";
                        }
                    }
                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            Classes.Coupon coupon = new Classes.Coupon();

                            coupon.Date = reader1.GetValue(0);


                            coupons.Add(coupon);
                            Date.Items.Add(coupon.Date);
                        }
                    }
                    reader1.Close();
                }
            }
            else if (DoctorName.Text == "Врач" && Spesname.Text != "Процедура")
            {
                string text = Spesname.Text;
                Date.Items.Clear();
                Time.Items.Clear();
                Spesname.Items.Clear();
                Spesname.Items.Add("Процедура");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlExpression2 = $"SELECT * FROM [Procedures] ";

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
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
                            Spesname.Items.Add(procedure.Name_Procedures);
                        }
                    }

                    for (int i = 0; i < Spesname.Items.Count; i++)
                    {
                        Spesname.SelectedIndex = i;
                        if (Spesname.Text == text)
                        { break; }
                    }

                    connection.Close();

                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    procedures.Clear();
                    DoctorName.Items.Clear();
                    DoctorName.Items.Add("Врач");
                    DoctorName.SelectedIndex = 0;
                    connection.Open();


                    string sqlExpression3 = $"SELECT Doctor.ID_Doctor,Doctor.Name_Doctor,Doctor.Surname_Doctor,Doctor.Middle_name_Doctor,Doctor.Photo_Doctor,Doctor.Photo_Doctor FROM Doctor " +
                            $"inner join [Procedures] on Doctor.Spezialization=[Procedures].Spezialization  and [Procedures].Name_Procedures='{Spesname.Text}'";

                    string comdobox;
                    SqlCommand sqlCommand = new SqlCommand(sqlExpression3, connection);
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
                            doctor.Photo_Doctor = reader.GetValue(4);
                            doctor.Photo_Doctor_byte = (byte[])reader.GetValue(4);
                            comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                            DoctorName.Items.Add(comdobox);
                            doctors.Add(doctor);
                        }
                    }

                    foto.Source = null;


                }


            }
            else if (DoctorName.Text == "Врач" && Spesname.Text == "Процедура")
            {
                foto.Source = null;
                Date.Items.Clear();
                Time.Items.Clear();
                DoctorName.Items.Clear();
                DoctorName.Items.Add("Врач");
                DoctorName.SelectedIndex = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = $"SELECT* FROM Doctor";
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
                                doctor.Photo_Doctor = reader.GetValue(7);
                                doctor.Photo_Doctor_byte = (byte[])reader.GetValue(7);
                                comdobox = (string)doctor.Surname_Doctor + " " + (string)doctor.Name_Doctor + " " + (string)doctor.Middle_name_Doctor;
                                DoctorName.Items.Add(comdobox);
                                doctors.Add(doctor);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                Spesname.Items.Clear();
                Spesname.Items.Add("Процедура");
                Spesname.SelectedIndex = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        string sqlExpression2 = $"SELECT * FROM Procedures";
                        SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
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
                                Spesname.Items.Add(procedure.Name_Procedures);
                            }
                        }
                        Spesname.SelectedIndex = 0;
                        reader.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

        }
        private void Date_DropDownClosed(object sender, EventArgs e)
        {

            Time.Items.Clear();
            coupons.Clear();
            if (Date.Text != "")
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression3 = null;
                    foreach (var doc in doctors)
                    {
                        if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                        {
                            sqlExpression3 = $"SELECT* from Coupon where Ordered='no' and ID_Doctor='{doc.ID_Doctor}' and Date=Convert(date, Right('{DateTime.Parse(Date.Text).ToShortDateString()}', 4)+SUBSTRING('{DateTime.Parse(Date.Text).ToShortDateString()}',4 , 2)+LEFT('{DateTime.Parse(Date.Text).ToShortDateString()}', 2))";
                        }
                    }

                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            Classes.Coupon coupon = new Classes.Coupon();

                            coupon.ID_Coupon = reader1.GetValue(0);
                            coupon.Date = reader1.GetValue(1);
                            coupon.time = reader1.GetValue(2);
                            coupon.ID_Doctor = reader1.GetValue(3);
                            coupon.Ordered = reader1.GetValue(4);

                            coupons.Add(coupon);
                            Time.Items.Add(coupon.time);
                        }
                    }
                    reader1.Close();
                    connection.Close();
                }

            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (Date.Text != "" && Time.Text != "" && DoctorName.Text != "Врач" && Spesname.Text != "Процедура")
            {
                int ID_D = 0;
                int ID_P = 0;
                foreach (var doc in doctors)
                {


                    if ((string)doc.Surname_Doctor + " " + (string)doc.Name_Doctor + " " + (string)doc.Middle_name_Doctor == DoctorName.Text)
                    {
                        ID_D = (int)doc.ID_Doctor;
                    }

                }
                foreach (var pro in procedures)
                {
                    if ((string)pro.Name_Procedures == Spesname.Text)
                    {
                        ID_P = (int)pro.ID_Procedures;
                    }
                }
                Classes.Coupon coupon = new Classes.Coupon();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression3 = null;

                    sqlExpression3 = $"SELECT* from Coupon where Ordered='no' and time='{Time.Text}' and ID_Doctor='{ID_D}' and Date=Convert(date, Right('{DateTime.Parse(Date.Text).ToShortDateString()}', 4)+SUBSTRING('{DateTime.Parse(Date.Text).ToShortDateString()}',4 , 2)+LEFT('{DateTime.Parse(Date.Text).ToShortDateString()}', 2))";


                    SqlCommand sqlCommand1 = new SqlCommand(sqlExpression3, connection);
                    SqlDataReader reader1 = sqlCommand1.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {


                            coupon.ID_Coupon = reader1.GetValue(0);
                            coupon.Date = reader1.GetValue(1);
                            coupon.time = reader1.GetValue(2);
                            coupon.ID_Doctor = reader1.GetValue(3);
                            coupon.Ordered = reader1.GetValue(4);
                        }
                    }
                    reader1.Close();
                    connection.Close();
                }
                string sql = $"INSERT INTO [Order] ([Date],[ID_Procedures],[ID_Client],[ID_Doctor],[ID_Coupon],[Time]) VALUES (Convert(date, Right('{DateTime.Parse(Date.Text).ToShortDateString()}', 4) + SUBSTRING('{DateTime.Parse(Date.Text).ToShortDateString()}', 4, 2) + LEFT('{DateTime.Parse(Date.Text).ToShortDateString()}', 2)),'{ID_P}','{client.ID_Clients}','{ID_D}','{coupon.ID_Coupon}','{Time.Text}')";
                string sql1 = $"UPDATE Coupon SET Ordered = 'yes' WHERE ID_Coupon = '{coupon.ID_Coupon}'";
                try
                {

                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command1 = new SqlCommand(sql, sqlConnection);
                        command1.ExecuteNonQuery();
                        SqlCommand command2 = new SqlCommand(sql1, sqlConnection);
                        command2.ExecuteNonQuery();
                        MessangeBox_Ok messange = new MessangeBox_Ok("Ваш заказ принят, проверьте свою почту!");
                        messange.Show();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587)
                    {
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("test@mail.ru", "test1234567890"),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress("test@mail.ru"),
                        Subject = "Ваш заказ принят",
                        Body = $"Здравствуйте, {client.Name_Client} {client.Surname_Client}.<br>" +
                               $"Ваш заказ принят. Вы записаны на <b>{Spesname.Text}</b>.<br>" +
                               $"Дата: {DateTime.Parse(Date.Text).ToShortDateString()}<br>" +
                               $"Время: {coupon.time}<br>" +
                               $"Ваш лечащий врач: {DoctorName.Text}.<br>" +
                               "Спасибо, что выбрали нас!",
                        IsBodyHtml = true
                    };
                    message.To.Add("test@mail.ru");

                    smtp.Send(message);
                    //MessageBox.Show("Письмо отправлено!");
                }
                catch (SmtpException ex)
                {
                    MessageBox.Show($"Ошибка SMTP: {ex.Message}");
                }


            }
            if (Time.Text == "")
            {
                Warring.Text = "Выберите Время";
            }
            if (Date.Text == "")
            {
                Warring.Text = "Выберите дату";
            }
            if (DoctorName.Text == "Врач")
            {
                Warring.Text = "Выберите Врача";
            }
            if (Spesname.Text == "Процедура")
            {
                Warring.Text = "Выберите Процедуру";
            }
        }
    }
}

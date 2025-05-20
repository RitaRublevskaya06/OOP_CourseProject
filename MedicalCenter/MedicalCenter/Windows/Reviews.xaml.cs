using MedicalCenter.ForUser;
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
    /// Логика взаимодействия для Reviews.xaml
    /// </summary>
    public partial class Reviews : Page
    {
        Classes.Clients client = new Classes.Clients();
        string connectionString;
        List<Classes.Review> selectreviews = new List<Classes.Review>();
        public Reviews()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            Reviewswrite.Width = 0;
            Reviewswrite.Height = 0;
            write.Width = 0;
            write.Height = 0;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();

        }
        public Reviews(ForAdmin.Admin admin)
        {

            InitializeComponent();
            Reviewswrite.Width = 0;
            Reviewswrite.Height = 0;
            write.Width = 0;
            write.Height = 0;
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
        }
        public Reviews(Classes.Clients clients)
        {
            client = clients;
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();


        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sqlExpression2 = $"SELECT Reviews.ID_Review,Reviews.ID_Client,Reviews.Heading, Reviews.Review,Clients.Image " +
                        $"FROM Reviews Inner join Clients on Reviews.ID_Client = Clients.ID_Client";

                    SqlCommand sqlCommand = new SqlCommand(sqlExpression2, connection);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<Classes.Review> reviews = new List<Classes.Review>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Classes.Review review = new Classes.Review();

                            review.ID_Review = reader.GetValue(0);
                            review.ID_Client = reader.GetValue(1);
                            review.Heading = reader.GetValue(2);
                            review.Description = reader.GetValue(3);
                            review.Photo_Client = reader.GetValue(4);
                            reviews.Add(review);

                        }
                    }


                    reader.Close();
                    Control.ItemsSource = reviews;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Reviewswrite_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddReviews addReviews = new AddReviews(client);
            addReviews.Show();
        }

    }
}

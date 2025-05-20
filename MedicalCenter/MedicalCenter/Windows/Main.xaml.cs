using MedicalCenter.ForDoctor;
using MedicalCenter.ForUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace MedicalCenter.Windows
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        Classes.Clients client;
        public Classes.Doctor Doctor { get; private set; }
        static public bool messange;
        public static object names;
        ForAdmin.Admin admin;
        public bool Admin = false;
        public bool guest = false;
        public bool user = false;
        private static bool _isFirstLoad = true;

        public Main()
        {
            InitializeComponent();
            guest = true;
            Allorder.Height = 0;
            Allorder.Width = 0;
            foradmin.Width = 0;
            foradmin.Height = 0;
        }
        public Main(Classes.Clients clients)
        {
            InitializeComponent();
            user = true;
            client = clients;

            // Скрываем элементы для клиента
            ConfirmService.Visibility = Visibility.Collapsed;
            imgConfirmService.Visibility = Visibility.Collapsed;
            AddPrescriptionBtn.Visibility = Visibility.Collapsed;
            imgAddPrescription.Visibility = Visibility.Collapsed;
            ConfirmServiceEllipse.Visibility = Visibility.Collapsed;
            AddPrescriptionEllipse.Visibility = Visibility.Collapsed;

            Allorder.Height = 0;
            Allorder.Width = 0;
            foradmin.Width = 0;
            foradmin.Height = 0;
        }
        public Main(ForAdmin.Admin admin)
        {
            InitializeComponent();
            Admin = true;
            // Разрешаем отображение кнопки "Все заказы"
            Allorder.Visibility = Visibility.Visible;
            // Убедитесь, что другие элементы не скрыты ошибочно
            StartButtonAnimations(); // Если анимация требуется

            // Скрываем элементы для врача
            ConfirmService.Visibility = Visibility.Collapsed;
            imgConfirmService.Visibility = Visibility.Collapsed;
            AddPrescriptionBtn.Visibility = Visibility.Collapsed;
            imgAddPrescription.Visibility = Visibility.Collapsed;
            ConfirmServiceEllipse.Visibility = Visibility.Collapsed;
            AddPrescriptionEllipse.Visibility = Visibility.Collapsed;
        }

        public bool IsDoctor { get; set; }


        public Main(Classes.Doctor doctor)
        {
            InitializeComponent();
            Doctor = doctor;
            IsDoctor = true;
            InitializeDoctorView();

        }


        private void InitializeDoctorView()
        {
            ConfirmService.Visibility = Visibility.Visible;
            imgConfirmService.Visibility = Visibility.Visible;
            AddPrescriptionBtn.Visibility = Visibility.Visible;
            imgAddPrescription.Visibility = Visibility.Visible;


            StartButtonAnimations();
        }
        private void StartButtonAnimations()
        {
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 400,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            foreach (var btn in new[] { ConfirmService, AddPrescriptionBtn, Ordercreate, Allorder })
            {
                btn?.BeginAnimation(WidthProperty, animation);
            }
        }


        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.ExtraData is Classes.Doctor doc)
            {
                Doctor = doc;
                IsDoctor = true;
                InitializeDoctorView();
            }
            // base.OnNavigatedTo(e); // Убрать вызов базового метода
        }



        private void Place1_Click(object sender, RoutedEventArgs e)
        {
            Manager.DoctorFraim.Navigate(new Aboutas());
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.DoctorFraim.Navigate(new Aboutas());
        }

        private void Place_Click(object sender, RoutedEventArgs e)
        {
            if (user)
            {
                Createorder createorder = new Createorder(client);
                createorder.Show();
            }
            if (Admin || IsDoctor) // Добавлено IsDoctor
            {
                MessangeBox_Ok messangeBox_OK1 = new MessangeBox_Ok("Эта функция недоступна!");
                messangeBox_OK1.Show();
            }
        }

        private void Allorder_Click(object sender, RoutedEventArgs e)
        {
            Manager.DoctorFraim.Navigate(new AllOrder());
        }

        private void foradmin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.DoctorFraim.Navigate(new AllOrder());
        }


        private void AddPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (IsDoctor && Doctor != null)
            {

                var prescriptionWindow = new AddPrescription(Doctor);
                prescriptionWindow.Owner = Window.GetWindow(this);
                prescriptionWindow.ShowDialog();
            }
        }
        private void ConfirmService_Click(object sender, RoutedEventArgs e)
        {
            if (IsDoctor && Doctor != null)
            {
                var confirmserviceWindow = new ConfirmService(Doctor);
                confirmserviceWindow.Owner = Window.GetWindow(this);
                confirmserviceWindow.ShowDialog();
            }
        }

        private void RefreshDoctorState()
        {
            InitializeDoctorView();
            StartButtonAnimations();
        }
        private void imgAddPrescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddPrescription_Click(sender, e);
        }

        private void imgConfirmService_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConfirmService_Click(sender, e);
        }
    }
}

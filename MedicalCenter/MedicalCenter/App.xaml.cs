using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MedicalCenter
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Проверка подключения к БД
            string connectionString = ConfigurationManager.ConnectionStrings["MedicalCenter.Properties.Settings.Corser_projectConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Подключение к базе данных успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            // Глобальный обработчик неотловленных исключений
            DispatcherUnhandledException += (sender, ex) =>
            {
                ex.Handled = true;
                MessageBox.Show($"Критическая ошибка: {ex.Exception.Message}");

                // Безопасное завершение приложения
                if (Current != null)
                {
                    Current.Shutdown();
                }
                else
                {
                    Environment.Exit(1); // Гарантированное завершение
                }
            };

            //// Создание и отображение главного окна
            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }
    }
}

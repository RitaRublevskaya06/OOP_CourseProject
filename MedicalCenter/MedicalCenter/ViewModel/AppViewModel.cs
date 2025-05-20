using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Configuration;

namespace MedicalCenter.ViewModel
{
    public class AppViewModel
    {
        public ICommand CloseAppCommand => new RelayCommand(CloseApp);

        private void CloseApp(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
    }

    //public class RelayCommand : ICommand
    //{
    //    private readonly Action<object> _execute;

    //    public RelayCommand(Action<object> execute) => _execute = execute;

    //    public bool CanExecute(object parameter) => true;

    //    public void Execute(object parameter) => _execute(parameter);

    //    public event EventHandler CanExecuteChanged;
    //}
}

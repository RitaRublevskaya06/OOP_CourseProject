using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Configuration; 
using System.IO;   

namespace MedicalCenter.Classes
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool boolValue && boolValue)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue; 
        }
    }
}

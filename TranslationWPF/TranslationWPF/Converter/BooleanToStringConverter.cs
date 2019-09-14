using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Data;

namespace TranslationWPF.Converter
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResourceManager rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());
            switch ((bool?)value)
            {
                case false:
                    return rm.GetString("correct");                   
                case true:
                    return rm.GetString("incorrect");
                case null:
                    return "";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

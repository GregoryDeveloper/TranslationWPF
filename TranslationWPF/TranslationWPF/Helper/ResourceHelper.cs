using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TranslationWPF.Services;

namespace TranslationWPF.Helper
{
    public static class ResourceHelper  
    {
        public static T GetResource<T>(string name)
        {
            return (T) Application.Current.Resources[name];
        }
    }
}

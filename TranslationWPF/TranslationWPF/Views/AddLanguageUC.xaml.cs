using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
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
using TranslationWPF.Model;
using TranslationWPF.ViewModel;

namespace TranslationWPF.Views
{
    /// <summary>
    /// Logique d'interaction pour AddLanguageUC.xaml
    /// </summary>
    public partial class AddLanguageUC : UserControl
    {
        ResourceManager rm;
        CultureInfo ci;

        public AddLanguageUC()
        {
            InitializeComponent();
            ci = Thread.CurrentThread.CurrentCulture;

            // TODO: hard coded string
            rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            AddLanguageVM viewmodel = (AddLanguageVM)DataContext;
            List<Language.Languages> languages = new List<Language.Languages>();

            languages.Add(viewmodel.SelectedItem1);
            languages.Add(viewmodel.SelectedItem2);

            //TODO: refacto navigation au même endroit
            DataContext = new EncodingVM(true, languages);
        }
    }
}

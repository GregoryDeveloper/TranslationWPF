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
using TranslationWPF.Views;

namespace TranslationWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ResourceManager rm;
        CultureInfo ci;
        List<Translation> translations = new List<Translation>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ci = Thread.CurrentThread.CurrentCulture;

            rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());

            translations.Add(new Translation(
                new French() { Value = "essayer" },
                new English() { Value = "to try" }));         
               
            translations.Add(new Translation(
                    new French()
                    {
                        Value = "manger",
                        Synonysms = { "dévorer", "déguster" }
                    },
                    new English()
                    {
                        Value = "to eat"
                    }));
            translations.Add(new Translation(
                    new French() { Value = "dormir" },
                    new English() { Value = "to sleep" }));
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            ImportView view = new ImportView();
            view.DataContext = new ImportVM(translations);
            view.Show();
            this.Close();
        }

        private void LBEncoding_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new EncodingVM(new French(), new English(),translations,rm,ci);
        }
        private void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new ModifyWordVM(translations, new EncodingVM(new French(), new English(), translations, rm, ci));
        }
    }
}

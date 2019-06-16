using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Principal;
using System.Threading;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using TranslationWPF.Helper;
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
            UserNameTB.Text = Environment.UserName;
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
            WelcomePage();

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

        void view_RaiseCustomEvent(object sender, CustomEventArgs e)
        {
            translations = e.Translations;
        }

        private void LBEncoding_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                List<Language.Languages> languages = PickUpLanguages();
                DataContext = new EncodingVM(new French(), new English(),translations,rm,ci,true, languages);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LBWelcome_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WelcomePage();
        }
        
        private void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            switch (item.Name)
            {
                case "LBModify":
                    try
                    {
                        List<Language.Languages> languages = PickUpLanguages();
                        DataContext = new ModifyWordVM(translations, new EncodingVM(new French(), new English(), translations, rm, ci, false, languages), languages);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                         break;
                case "LBPratice":
                    DataContext = new TrainingsVM(translations, rm, ci);
                    break;
                default:
                    break;
            }
        }

        private List<Language.Languages> PickUpLanguages()
        {
            LanguagePickupWindow window = new LanguagePickupWindow();

            PickupVM pickup = new PickupVM(translations[0]);
            window.DataContext = pickup;
            window.ShowDialog();
            List<Language.Languages> languages = new List<Language.Languages>();
            languages.Add(pickup.SelectedItem1);
            languages.Add(pickup.SelectedItem2);
            return languages;
        }

        private void WelcomePage()
        {
            try
            {
                // link to the corresponding using done in MainWindow.xaml in window.resources
                DataContext = new WelcomeVM(translations,rm, ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}

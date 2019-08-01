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
using TranslationWPF.Exceptions;
using TranslationWPF.Helper;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;
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
        //List<Translation> translations = new List<Translation>();
        TranslationService translationService;
        public MainWindow()
        {
            InitializeComponent();
            this.translationService = new TranslationService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserNameTB.Text = Environment.UserName;
            ci = Thread.CurrentThread.CurrentCulture;

            rm = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());

            translationService.AddTranslation(new Translation(
                new French() { Value = "essayer" },
                new English() { Value = "to try" }));

            translationService.AddTranslation(new Translation(
                    new French()
                    {
                        Value = "manger",
                        Synonysms = { "dévorer", "déguster" }
                    },
                    new English()
                    {
                        Value = "to eat"
                    }));
            translationService.AddTranslation(new Translation(
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
            translationService.Translations = e.Translations;
        }

        private void LBEncoding_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                List<Language.Languages> languages = EncodingPickUpLanguages();
                DataContext = new EncodingVM(translationService, rm,ci,true, languages);
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
                        DataContext = new ModifyWordVM(translationService, new EncodingVM(translationService, rm, ci, false, languages), languages, rm, ci);
                        ((ModifyWordVM)DataContext).TranslationService = this.translationService;
                        //translationService.Translations = ((ModifyWordVM)DataContext).TranslationsModel;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                    break;

                case "LBPratice":
                    try
                    {
                        DataContext = new TrainingsVM(translationService, rm, ci);
                    }
                    catch (Exception ex )
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                default:
                    break;
            }
        }

        private List<Language.Languages> PickUpLanguages()
        {
            if (translationService.Translations.Count == 0)
                throw new NoItemException(rm.GetString(StringConstant.noItemExceptionMessage, ci));

            LanguagePickupWindow window = new LanguagePickupWindow();

            PickupVM pickup = new PickupVM(translationService.Translations[0],rm,ci);
            window.DataContext = pickup;
            window.ShowDialog();
            List<Language.Languages> languages = new List<Language.Languages>();
            languages.Add(pickup.SelectedItem1);
            languages.Add(pickup.SelectedItem2);
            return languages;
        }

        // TODO refactring
        private List<Language.Languages> EncodingPickUpLanguages()
        {
            LanguagePickupWindow window = new LanguagePickupWindow();

            PickupVM pickup = new PickupVM( rm, ci);
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
                DataContext = new WelcomeVM(translationService.Translations, rm, ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}

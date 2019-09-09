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
        // TODO : remove rm,ci and translationService from all the constructors
        ResourceManager rm;
        CultureInfo ci;
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

            WelcomePage();

        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            if (translationService.IsDirty)
            {
                var message = rm.GetString(StringConstant.leaveMessageBox, ci);
                var caption = rm.GetString(StringConstant.leaveMessageBoxCaption, ci);

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    Application.Current.Shutdown();
            }
            else
            {
                Application.Current.Shutdown();
            }
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

        private void LBEncoding_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                List<Language.Languages> languages = EncodingPickUpLanguages();
                DataContext = new EncodingVM(translationService, rm,ci,true, languages);
                translationService.LanguagesOrder = languages;
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
                        List<Language.Languages> languages = PickupLanguageHelper.PickUpLanguages(translationService,rm,ci);
                        DataContext = new ModifyWordVM(translationService, new EncodingVM(translationService, rm, ci, false, languages), languages, rm, ci);
                        
                       
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                    break;

                case "LBPratice":
                    try
                    {
                        List<Language.Languages> languages = PickupLanguageHelper.PickUpLanguages(translationService, rm, ci);

                        DataContext = new TrainingsVM(translationService, rm, ci, languages[0], languages[1]);

                    }
                    catch (Exception ex )
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                case "LBAddLanguage":
                    var dataContext = new AddLanguageVM(rm, ci, translationService);
                    dataContext.ChangementEtat += UCClosed;
                    DataContext = dataContext;

                    break;
                default:
                    break;
            }
        }

        private void UCClosed(object sender, (string Name, List<Language.Languages> Languages, TranslationVM translation) e)
        {
            DataContext = new EncodingVM(translationService, rm, ci, true, e.Languages, e.translation);
            translationService.LanguagesOrder = e.Languages;
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
                DataContext = new WelcomeVM(translationService, rm, ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

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
            //EncodingPickUpLanguages();

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
            List<Language.Languages> languages = new List<Language.Languages>()
            {
                TranslationWPF.Model.Language.Languages.French,
                TranslationWPF.Model.Language.Languages.English
            };
            ImportView view = new ImportView();
            ImportVM importView = new ImportVM(translations, languages,rm,ci);
            view.DataContext = importView;
            view.RaiseCustomEvent += new EventHandler<CustomEventArgs>(view_RaiseCustomEvent);
            view.Show();
            //this.Close();
        }

        void view_RaiseCustomEvent(object sender, CustomEventArgs e)
        {
            translations = e.Translations;
        }

        private void LBEncoding_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            List<string> languages = EncodingPickUpLanguages();
            DataContext = new EncodingVM(new French(), new English(),translations,rm,ci,true, languages);
        }
        private void ListViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            switch (item.Name)
            {
                case "LBModify":
                    try
                    {
                        List<string> languages = PickUpLanguages();
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

        private List<string> PickUpLanguages()
        {
            LanguagePickupWindow window = new LanguagePickupWindow();

            PickupVM pickup = new PickupVM(translations[0]);
            window.DataContext = pickup;
            window.ShowDialog();
            List<string> languages = new List<string>();
            languages.Add(pickup.SelectedItem1.ToDescription());
            languages.Add(pickup.SelectedItem2.ToDescription());
            return languages;
        }

        private List<string> EncodingPickUpLanguages()
        {
            EncodingLanguagePickUpWindow window = new EncodingLanguagePickUpWindow();

            EncodingLanguagePickUpVM pickup = new EncodingLanguagePickUpVM(translations[0]);
            window.DataContext = pickup;
            window.ShowDialog();
            List<string> languages = new List<string>();
            languages.Add(pickup.SelectedItem1);
            languages.Add(pickup.SelectedItem2);
            return languages;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TranslationWPF.Model;
using TranslationWPF.Views;

namespace TranslationWPF.ViewModel
{
    public class WelcomeVM
    {
        #region Properties

        #region UI Properties
        private string title;
        public string Title { get { return title = title ?? rm.GetString("title", ci); } }

        private string importbtn;
        public string Importbtn { get { return importbtn = importbtn ?? rm.GetString("import", ci); } }
        
        private string leavebtn;
        public string Leavebtn { get { return leavebtn = leavebtn ?? rm.GetString("leave", ci); } }
        #endregion

        #region Properties
        private ResourceManager rm;
        private CultureInfo ci;
        public List<Translation> Translations { get; set; }
        #endregion

        #endregion

        public WelcomeVM(List<Translation> translations, ResourceManager rm, CultureInfo ci)
        {
            this.rm = rm;
            this.ci = ci;
            Translations = translations;
        }

        #region Commands
        private CommandHandler _importCommand;
        public CommandHandler ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new CommandHandler(() => ImportHandler(), true)); }
        }

        private CommandHandler _leaveCommand;
        public CommandHandler LeaveCommand
        {
            get { return _leaveCommand ?? (_leaveCommand = new CommandHandler(() => LeaveHandler(), true)); }
        }
        #endregion

        #region Handlers
        private void ImportHandler()
        {
            List<Language.Languages> languages = new List<Language.Languages>()
                {
                    TranslationWPF.Model.Language.Languages.French,
                    TranslationWPF.Model.Language.Languages.English
                };
            ImportView view = new ImportView();
            ImportVM importView = new ImportVM(Translations, languages, rm, ci);
            view.DataContext = importView;
            view.RaiseCustomEvent += new EventHandler<CustomEventArgs>(view_RaiseCustomEvent);
            view.Show();
            //this.Close();
        }

        private void LeaveHandler()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Events
            void view_RaiseCustomEvent(object sender, CustomEventArgs e)
            {
                foreach (Translation translation in e.Translations)
                {
                    Translations.Add(translation);
                }
            }
        #endregion
    }
}

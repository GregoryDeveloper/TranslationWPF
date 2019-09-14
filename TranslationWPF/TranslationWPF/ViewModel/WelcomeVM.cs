using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Windows;
using TranslationWPF.Helper;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;
using TranslationWPF.Views;

namespace TranslationWPF.ViewModel
{
    public class WelcomeVM
    {
        #region Properties

        #region UI Properties
        private string title;
        public string Title { get { return title = title ?? rm.GetString(StringConstant.title, ci); } }

        private string importbtn;
        public string Importbtn { get { return importbtn = importbtn ?? rm.GetString(StringConstant.import, ci); } }
        
        private string leavebtn;
        public string Leavebtn { get { return leavebtn = leavebtn ?? rm.GetString(StringConstant.leave, ci); } }

        private string clearbtn;
        public string Clearbtn { get { return clearbtn = clearbtn ?? rm.GetString(StringConstant.clear, ci); } }

        #endregion

        #region Properties
        private ResourceManager rm;
        private CultureInfo ci;
        private TranslationService translationService;
        //public List<Translation> Translations { get; set; }
        #endregion

        #endregion

        public WelcomeVM()
        {
            this.rm = LanguageSingleton.Instance.ResourceManager;
            this.ci = LanguageSingleton.Instance.CultureInfo;
            this.translationService = ResourceHelper.GetResource<TranslationService>(Constants.TRANSLATION_SERVICE);
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

        private CommandHandler _clearCommand;
        public CommandHandler ClearCommand
        {
            get { return _clearCommand ?? (_clearCommand = new CommandHandler(() => ClearHandler(), true)); }
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
            ImportVM importView = new ImportVM(languages);
            view.DataContext = importView;
           
            view.Show();
        }

        private void LeaveHandler()
        {
            if (translationService.IsDirty)
            {
                var message = rm.GetString(StringConstant.leaveMessageBox, ci);
                var caption = rm.GetString(StringConstant.leaveMessageBoxCaption, ci);

                MessageBoxResult result =  MessageBox.Show(message, caption,MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    Application.Current.Shutdown();

            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void ClearHandler()
        {
            translationService.Clear();
        }

        #endregion

    }
}

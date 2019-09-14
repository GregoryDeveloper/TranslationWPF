using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using TranslationWPF.Languages;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class PickupVM
    {
        #region Properties

        #region UIProperties
        private string language1;
        public string Language1 { get { return language1 = language1 ?? rm.GetString(StringConstant.language, ci) + " 1: "; } }
        private string language2;
        public string Language2 { get { return language2 = language2 ?? rm.GetString(StringConstant.language, ci) + " 2: "; } }

        #endregion

        readonly ResourceManager rm;
        readonly CultureInfo ci;

        public ObservableCollection<Language.Languages> LanguagesOrder { get; set; } = new ObservableCollection<Language.Languages>();
        public Language.Languages SelectedItem1 { get; set; }
        public Language.Languages SelectedItem2 { get; set; }
        #endregion 

        public PickupVM(List<Language.Languages> Languages)
        {
            foreach (var item in Languages)
            {
                LanguagesOrder.Add(item);
            }

            this.rm = LanguageSingleton.Instance.ResourceManager;
            this.ci = LanguageSingleton.Instance.CultureInfo;
        }

        //TODO refactoring
        public PickupVM()
        {

            var languages = Enum.GetValues(typeof(Language.Languages)).Cast<Language.Languages>();

            foreach (var item in languages)
            {
                LanguagesOrder.Add(item);
            }

            this.rm = LanguageSingleton.Instance.ResourceManager;
            this.ci = LanguageSingleton.Instance.CultureInfo;
        }

        private CommandHandler closingCommand;
        public CommandHandler ClosingCommand
        {
            get { return closingCommand ?? (closingCommand = new CommandHandler(() => Closing(), true)); }

        }
        
        private void Closing()
        {

        }
    }
}

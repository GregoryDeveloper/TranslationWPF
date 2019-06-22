using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
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

        // TODO :   ResourceManager rm, CultureInfo ci
        public PickupVM(Translation translation, ResourceManager rm, CultureInfo ci)
        {
            foreach (Language item in translation.Languages)
            {
                LanguagesOrder.Add(item.GetLanguage());
            }

            this.rm = rm;
            this.ci = ci;
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

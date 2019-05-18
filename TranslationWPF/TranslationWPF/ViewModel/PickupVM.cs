using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class PickupVM
    {
        public ObservableCollection<Language.Languages> LanguagesOrder { get; set; } = new ObservableCollection<Language.Languages>();
        public Language.Languages SelectedItem1 { get; set; }
        public Language.Languages SelectedItem2 { get; set; }

        public PickupVM(Translation translation)
        {
            foreach (Language item in translation.Languages)
            {
                LanguagesOrder.Add(item.GetLanguage());
            }
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

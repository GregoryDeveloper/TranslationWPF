using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Helper;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class EncodingLanguagePickUpVM : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public ObservableCollection<string> LanguagesOrder { get; set; } = new ObservableCollection<string>();
        public string SelectedItem1 { get; set; }
        public string SelectedItem2 { get; set; }
        private string languageToAdd;
        public string LanguageToAdd
        {
            get { return languageToAdd; }
            set { languageToAdd = value; OnPropertyChanged("LanguageToAdd"); }
        }

        public EncodingLanguagePickUpVM(Translation translation)
        {
            foreach (Language item in translation.Languages)
            {
                LanguagesOrder.Add(item.GetLanguage().ToDescription());
            }
        }

        private CommandHandler _addCommand;
        public CommandHandler AddCommand
        {
            get { return _addCommand ?? (_addCommand = new CommandHandler(() => AddHandler(), true)); }
        }

        private CommandHandler closingCommand;
        public CommandHandler ClosingCommand
        {
            get { return closingCommand ?? (closingCommand = new CommandHandler(() => Closing(), true)); }
        }

        private void Closing()
        {

        }

        void AddHandler()
        {
            if (!string.IsNullOrEmpty(LanguageToAdd))
            {
                if (!LanguagesOrder.Contains(LanguageToAdd))
                {
                    LanguagesOrder.Add(LanguageToAdd);
                }
                LanguageToAdd = "";
            }         

        }
    }
}

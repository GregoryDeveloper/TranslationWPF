using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class EncodingVM: INotifyPropertyChanged
    {
        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        public Language Word { get; set; }
        public Language Translation { get; set; }

        private string wordAddingSynonym ="";
        public string WordAddingSynonym
        {
            get { return wordAddingSynonym;}
            set { wordAddingSynonym = value; OnPropertyChanged("WordAddingSynonym");}
        }

        private string translationAddingSynonym="";
        public string TranslationAddingSynonym
        {
            get { return translationAddingSynonym; }
            set { translationAddingSynonym = value; OnPropertyChanged("TranslationAddingSynonym"); }
        }

        public Language.Types[] WordTypes
        {
            get { return Word.GetTypesAvailables();}
        }
        public Language.Types[] TranslationTypes
        {
            get { return Translation.GetTypesAvailables(); }
        }

        private Language.Types wordSelectedType;
        public Language.Types WordSelectedType
        {
            get { return wordSelectedType; }
            set { wordSelectedType = value; OnPropertyChanged("WordSelectedType"); }
        }

        private Language.Types translationSelectedType;
        public Language.Types TranslationSelectedType
        {
            get { return translationSelectedType; }
            set { translationSelectedType = value; OnPropertyChanged("TranslationSelectedType"); }
        }


        public ObservableCollection<string> OriginalWordSynonyms { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> TranslatedWordSynonyms { get; set; } = new ObservableCollection<string>();
        #endregion

        #region Constructors
        public EncodingVM(Language word, Language translation)
        {
            Word = word;
            Translation = translation;
        }
        #endregion

        #region Commands
        private CommandHandlerWithParameter _removeWordCommand;
        public CommandHandlerWithParameter RemoveWordCommand
        {
            get { return _removeWordCommand ?? (_removeWordCommand = new CommandHandlerWithParameter(item => RemoveWordHandler((string)item), true)); }
        }

        private CommandHandlerWithParameter _removeTranslationCommand;
        public CommandHandlerWithParameter RemoveTranslationCommand
        {
            get { return _removeTranslationCommand ?? (_removeTranslationCommand = new CommandHandlerWithParameter(item => RemoveTranslationHandler((string)item), true)); }
        }

        private CommandHandlerWithParameter _addCommand;
        public CommandHandlerWithParameter AddCommand
        {
            get { return _addCommand ?? (_addCommand = new CommandHandlerWithParameter((item) => AddHandler((string) item), true)); }
            
        }

        #endregion

        #region Methods
        void RemoveWordHandler(string item)
        {
            string itemToRemove = OriginalWordSynonyms.First(w => w == item);
            OriginalWordSynonyms.Remove(itemToRemove);
        }
        void RemoveTranslationHandler(string item)
        {
            string itemToRemove = TranslatedWordSynonyms.First(w => w == item);
            TranslatedWordSynonyms.Remove(itemToRemove);
        }
        void AddHandler(string item)
        {
            switch (item)
            {
                case "1":
                    if (!string.IsNullOrEmpty(WordAddingSynonym))
                        OriginalWordSynonyms.Add(WordAddingSynonym);
                    break;
                case "2":
                    if (!string.IsNullOrEmpty(TranslationAddingSynonym))
                        TranslatedWordSynonyms.Add(TranslationAddingSynonym);
                    break;
                default:
                    break;
            }
            WordAddingSynonym = "";
            TranslationAddingSynonym = "";
        }
        #endregion
    }
}

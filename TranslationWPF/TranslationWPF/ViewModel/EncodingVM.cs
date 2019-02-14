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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Language Word { get; set; }
        public Language Translation { get; set; }

        public Language.Types[] Types
        {
            get
            {
                return Word.GetTypesAvailables();
            }
        }

        public ObservableCollection<string> OriginalWordSynonyms { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> TranslatedWordSynonyms { get; set; } = new ObservableCollection<string>();

        private Language.Types selectedType;
        public Language.Types SelectedType
        {
            get { return selectedType; }
            set { selectedType = value; OnPropertyChanged("SelectedType"); }
        }

        public EncodingVM(Language word, Language translation)
        {
            Word = word;
            Translation = translation;
        }

        #region Commands
        private CommandHandlerWithParameter _removeCommand;
        public CommandHandlerWithParameter RemoveCommand
        {
            get { return _removeCommand ?? (_removeCommand = new CommandHandlerWithParameter(item => RemoveHandler((string)item), true)); }
        }

        private CommandHandlerWithParameter _addCommand;
        public CommandHandlerWithParameter AddCommand
        {
            get { return _addCommand ?? (_addCommand = new CommandHandlerWithParameter(item => AddHandler((string)item), true)); }
            
        }

        private CommandHandlerWithParameter _selectTypeCommand;

       

        public CommandHandlerWithParameter SelectTypeCommand
        {
            get { return _selectTypeCommand ?? (_selectTypeCommand = new CommandHandlerWithParameter(item => SelectTypeHandler((Language.Types)item), true)); }
        }
        #endregion

        void RemoveHandler(string item)
        {
            string itemToRemove = OriginalWordSynonyms.Single(w => w == item);
            OriginalWordSynonyms.Remove(itemToRemove);
            //foreach (var word in OriginalWordSynonyms)
            //{
            //    if (word == item)
            //        OriginalWordSynonyms.Remove(item);
            //}
        }

        void AddHandler(string word)
        {
            OriginalWordSynonyms.Add(word);
        }

        void SelectTypeHandler(Language.Types type)
        {
            SelectedType = type;
        }

    }
}

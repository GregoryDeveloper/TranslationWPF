using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using TranslationWPF.DataValidation;
using TranslationWPF.Helper;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class EncodingVM : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        List<Translation> translations;
        ResourceManager rm;
        CultureInfo ci;
        // TODO unable the user to add a synonyms that is already in the list and pop up a notification message
        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyDataErrorInfo
        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) || (!HasErrors))
                return null;
            return new List<string>() { "Invalid credentials" };
        }
        public bool HasErrors { get; set; } = false;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool CheckCredentials()
        {
            
            HasErrors = !new ValueValidation().IsValid(Word.Value, Translation.Value);
            if (HasErrors)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("Word"));
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs("Translation"));
            }
            else
                return true;
            return false;
        }
        #endregion

        #region Properties

        #region UI properties
        private string UIword;
        public string UIWord
        {
            get { return UIword = (UIword == null) ? rm.GetString("word", ci) + ":" : UIword; }
        }

        private string UItranslation;
        public string UITranslation
        {
            get { return UItranslation = (UItranslation == null) ? rm.GetString("translation", ci) + ":" : UItranslation; }
        }

        private string UIcomment;
        public string UIComment
        {
            get { return UIcomment = (UIcomment == null) ? rm.GetString("comment", ci) + ":" : UIcomment; }
        }

        private string UIexemple;
        public string UIExemple
        {
            get { return UIexemple = (UIexemple == null) ? rm.GetString("exemple", ci) + ":" : UIexemple; }
        }

        private string UIsynonysms;
        public string UISynonysms
        {
            get { return UIsynonysms = (UIsynonysms == null) ? rm.GetString("synonysms", ci) + ":" : UIsynonysms; }
        }

        private string UItype;
        public string UIType
        {
            get { return UItype = (UItype == null) ? rm.GetString("type", ci) + ":" : UItype; }
        }

        private string UIaddButton;
        public string UIAddButton
        {
            get { return UIaddButton = (UIaddButton == null) ? rm.GetString("addToList", ci) : UIaddButton; }
        }

        private string UIadd;
        public string UIAdd
        {
            get { return UIadd = (UIadd == null) ? rm.GetString("add", ci) : UIadd; }
        }

        private string UIdelete;
        public string UIDelete
        {
            get { return UIdelete = (UIdelete == null) ? rm.GetString("delete", ci) : UIdelete; }
        }



        #endregion

        #region Other properties

        private Language word;
        public Language Word
        {
            get { return word; }
            set
            {
                word = value;
                OnPropertyChanged("Word");
                //CheckCredentials();
            }
        }
        private Language translation;
        public Language Translation
        {
            get { return translation; }
            set { translation = value;
                OnPropertyChanged("Translation");
                //CheckCredentials();
            }
        }

        private string wordAddingSynonym = "";
        public string WordAddingSynonym
        {
            get { return wordAddingSynonym; }
            set { wordAddingSynonym = value; OnPropertyChanged("WordAddingSynonym"); }
        }

        private string translationAddingSynonym = "";
        public string TranslationAddingSynonym
        {
            get { return translationAddingSynonym; }
            set { translationAddingSynonym = value; OnPropertyChanged("TranslationAddingSynonym"); }
        }

        public Language.Types[] WordTypes
        {
            get { return Word.GetTypesAvailables(); }
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

        

        #endregion

        #region Constructors
        public EncodingVM(Language _word, Language _translation, List<Translation> _translations, ResourceManager rm, CultureInfo ci)
        {
            word = _word;
            translation = _translation;
            translations = _translations;
            this.rm = rm;
            this.ci = ci;
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
            get { return _addCommand ?? (_addCommand = new CommandHandlerWithParameter((item) => AddHandler((string)item), true)); }

        }

        private CommandHandler _addWordCommand;
        public CommandHandler AddWordCommand
        {
            get { return _addWordCommand ?? (_addWordCommand = new CommandHandler(() => AddWordHandler(), true)); }

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
                    {
                        OriginalWordSynonyms.Add(WordAddingSynonym);
                        WordAddingSynonym = "";
                    }
                    break;
                case "2":
                    if (!string.IsNullOrEmpty(TranslationAddingSynonym))
                    {
                        TranslatedWordSynonyms.Add(TranslationAddingSynonym);
                        TranslationAddingSynonym = "";
                    }
                    break;
                default:
                    break;
            }

        }

        void AddWordHandler()
        {
            if (!CheckCredentials())
                return;

            Word.Type = wordSelectedType;
            Word.Synonysms = OriginalWordSynonyms.ToList();

            Translation.Type = TranslationSelectedType;
            Translation.Synonysms = TranslatedWordSynonyms.ToList();

            Translation translation = new Translation(Word, Translation);

            translations.Add(translation);
            ResetUI();
        }
        void ResetUI()
        {
            Word = Word.GetNewInstance();
            Translation = Translation.GetNewInstance();
            OriginalWordSynonyms.Clear();
            TranslatedWordSynonyms.Clear();

        }


        #endregion


    }
}

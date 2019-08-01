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
using TranslationWPF.Exceptions;
using TranslationWPF.Helper;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;

namespace TranslationWPF.ViewModel
{
    public class EncodingVM : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        //List<Translation> translations;
        public TranslationService TranslationService{ get; set; }

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

            HasErrors = !new ValueValidation().IsValid(Translation.Language1.Value, Translation.Language2.Value);
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

        public string UILanguage1 { get; }
        public string UILanguage2 { get; }
 

        private string UIword;
        public string UIWord
        {
            get { return UIword = (UIword == null) ? rm.GetString(StringConstant.word, ci) + ":" : UIword; }
        }

        private string UItranslation;
        public string UITranslation
        {
            get { return UItranslation = (UItranslation == null) ? rm.GetString(StringConstant.translation, ci) + ":" : UItranslation; }
        }

        private string UIcomment;
        public string UIComment
        {
            get { return UIcomment = (UIcomment == null) ? rm.GetString(StringConstant.comment, ci) + ":" : UIcomment; }
        }

        private string UIexemple;
        public string UIExemple
        {
            get { return UIexemple = (UIexemple == null) ? rm.GetString(StringConstant.exemple, ci) + ":" : UIexemple; }
        }

        private string UIsynonysms;
        public string UISynonysms
        {
            get { return UIsynonysms = (UIsynonysms == null) ? rm.GetString(StringConstant.synonysms, ci) + ":" : UIsynonysms; }
        }

        private string UItype;
        public string UIType
        {
            get { return UItype = (UItype == null) ? rm.GetString(StringConstant.type, ci) + ":" : UItype; }
        }

        private string UIaddButton;
        public string UIAddButton
        {
            get { return UIaddButton = (UIaddButton == null) ? rm.GetString(StringConstant.addToList, ci) : UIaddButton; }
        }

        public bool IsVisible { get; set; }

        private string UIadd;
        public string UIAdd
        {
            get { return UIadd = (UIadd == null) ? rm.GetString(StringConstant.add, ci) : UIadd; }
        }

        private string UIdelete;
        public string UIDelete
        {
            get { return UIdelete = (UIdelete == null) ? rm.GetString(StringConstant.delete, ci) : UIdelete; }
        }

        #endregion

        #region Other properties

        private TranslationVM translation;
        public TranslationVM Translation
        {
            get { return translation; }
            set { translation = value; OnPropertyChanged("Translation"); }
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

        #endregion

        #endregion

        #region Constructors
        public EncodingVM(  TranslationService  _translationsService, 
                            ResourceManager rm, 
                            CultureInfo ci, 
                            bool displayAddButton, 
                            List<Language.Languages> languages)
        {

            Translation t = new Translation(Language.CreateLanguage(languages[0]), Language.CreateLanguage(languages[1]));

            Translation = new TranslationVM(t,languages);
            TranslationService = _translationsService;
            this.rm = rm;
            this.ci = ci;
            IsVisible = displayAddButton;

            UILanguage1 = languages[0].ToDescription();
            UILanguage2 = languages[1].ToDescription();
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

        private CommandHandlerWithParameter _addWordCommand;
        public CommandHandlerWithParameter AddWordCommand
        {
            get { return _addWordCommand ?? (_addWordCommand = new CommandHandlerWithParameter((item) => AddWordHandler((string)item), true)); }

        }



        #endregion

        #region Methods
        public void SetItem(TranslationVM translation)
        {
            Translation = translation;
        }

        void RemoveWordHandler(string item)
        {
            string itemToRemove = Translation.Language1Synonyms.First(w => w == item);
            Translation.Language1Synonyms.Remove(itemToRemove);
        }
        void RemoveTranslationHandler(string item)
        {
            string itemToRemove = Translation.Language2Synonyms.First(w => w == item);
            Translation.Language2Synonyms.Remove(itemToRemove);
        }
        void AddHandler(string item)
        {
            switch (item)
            {
                case "1":
                    if (!string.IsNullOrEmpty(WordAddingSynonym))
                    {
                        Translation.Language1Synonyms.Add(WordAddingSynonym);
                        WordAddingSynonym = "";
                    }
                    break;
                case "2":
                    if (!string.IsNullOrEmpty(TranslationAddingSynonym))
                    {
                        Translation.Language2Synonyms.Add(TranslationAddingSynonym);
                        TranslationAddingSynonym = "";
                    }
                    break;
                default:
                    break;
            }

        }

        void AddWordHandler(string item)
        {
            if (!CheckCredentials())
                return;

            AddItemToList();
        }

        void AddItemToList()
        {
            // TODO: check if we can remove
            Translation.Save();

            TranslationService.AddTranslation(new TranslationVM(Translation));
            ResetUI();
        }

        void ResetUI()
        {
            Translation.Language1 = Translation.Language1.GetNewInstance();
            Translation.Language2 = Translation.Language2.GetNewInstance();
            Translation.Language1Synonyms.Clear();
            Translation.Language2Synonyms.Clear();

        }

        #endregion


    }
}

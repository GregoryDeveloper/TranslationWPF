using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using TranslationWPF.Languages;
using TranslationWPF.Model;
using TranslationWPF.Services;

namespace TranslationWPF.ViewModel
{
    public class AddLanguageVM : INotifyPropertyChanged
    {
        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region UIProperties
        private string baseLanguage;
        public string BaseLanguage { get { return baseLanguage = baseLanguage ?? rm.GetString(StringConstant.baseLanguage, ci); } }
        private string addingLanguage;
        public string AddingLanguage { get { return addingLanguage = addingLanguage ?? rm.GetString(StringConstant.addingLanguage, ci); } }
        #endregion  


        public ObservableCollection<Language.Languages> BaseLanguages { get; set; } = new ObservableCollection<Language.Languages>();
        public ObservableCollection<Language.Languages> NewLanguages { get; set; } = new ObservableCollection<Language.Languages>();
        private ObservableCollection<AddTranslationVM> translations;

        public ObservableCollection<AddTranslationVM> Translations
        {
            get { return translations; }
            set { translations = value; OnPropertyChanged("Translations"); }
        }



        private Language.Languages selectedItem1;
        public Language.Languages SelectedItem1
        {
            get { return selectedItem1; }
            set { selectedItem1 = value; OnPropertyChanged("SelectedItem1"); }
        }

        private Language.Languages selectedItem2;
        public Language.Languages SelectedItem2
        {
            get { return selectedItem2; }
            set { selectedItem2 = value; OnPropertyChanged("SelectedItem2"); }
        }

        private AddTranslationVM dgSelectedItem;
        public AddTranslationVM DGSelectedItem
        {
            get { return dgSelectedItem; }
            set { dgSelectedItem = value; OnPropertyChanged("DGSelectedItem"); }
        }

        public  TranslationService TranslationService { get; set; }


        readonly ResourceManager rm;
        readonly CultureInfo ci;
        public event EventHandler<(string, List<Language.Languages>, Translation translation)> ChangementEtat;

        public AddLanguageVM(ResourceManager rm, 
                             CultureInfo ci,
                             TranslationService translationService)
        {
            this.rm = rm;
            this.ci = ci;
            this.TranslationService = translationService;
            
            this.BaseLanguages = new ObservableCollection<Language.Languages> (Language.GetLanguages());
            
        }

        private CommandHandler selectionChangedCommand;
        public CommandHandler SelectionChangedCommand
        {
            get { return selectionChangedCommand ?? (selectionChangedCommand = new CommandHandler(() => SelectionChangedHandler(), true)); }
        }

        private CommandHandler datagridSelectionChangedCommand;
        public CommandHandler DatagridSelectionChangedCommand
        {
            get { return datagridSelectionChangedCommand ?? (datagridSelectionChangedCommand = new CommandHandler(() => DatagridSelectionChangedHandler(), true)); }
        }

        private CommandHandler validateChangedCommand;
        public CommandHandler ValidateChangedCommand
        {
            get { return validateChangedCommand ?? (validateChangedCommand = new CommandHandler(() => DatagridSelectionChangedHandler(), true)); }
        }

        private CommandHandler okCommand;
        public CommandHandler OKCommand
        {
            get { return okCommand ?? (okCommand = new CommandHandler(() => OKHandler(), true)); }
        }


        private void SelectionChangedHandler()
        {
            NewLanguages.Clear();
            List<Translation> translations = TranslationService.GetTranslations(SelectedItem1);
            Translations = new ObservableCollection<AddTranslationVM>();
            foreach (Translation translation in translations)
            {
                Translations.Add(new AddTranslationVM(translation, SelectedItem1));
            }

        }

        private void DatagridSelectionChangedHandler()
        {
            if (DGSelectedItem == null)
                return;

            NewLanguages.Clear();

            foreach (var item in DGSelectedItem.Translation.GetMissingLanguages())
            {
                NewLanguages.Add(item);
            }
        }

        private void OKHandler()
        {
            List<Language.Languages> languages = new List<Language.Languages>();
            languages.Add(SelectedItem1);
            languages.Add(SelectedItem2);

            DGSelectedItem.Translation.Languages.Add(Language.CreateLanguage(SelectedItem2));

            OnChangementEtat(("AddLanguageVM", languages, DGSelectedItem.Translation));
        }


        public void OnChangementEtat((string, List<Language.Languages>, Translation translation) e)
        {
            this.ChangementEtat?.Invoke(this, e);
        }
    }
}

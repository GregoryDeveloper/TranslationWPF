using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;
using System.Collections.ObjectModel;
using TranslationWPF.Helper;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TranslationWPF.Languages;
using System.Resources;
using System.Globalization;

namespace TranslationWPF.ViewModel
{
    public class ModifyWordVM: INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        #region UI

        public string UILanguage1 { get; }
        public string UILanguage2 { get; }
        private string UIdelete;
        public string UIDelete { get { return UIdelete = UIdelete ?? rm.GetString(StringConstant.delete, ci); } }

        #endregion  

        private readonly ResourceManager rm;
        private readonly CultureInfo ci;

        private ObservableCollection<TranslationVM> translations;
        public ObservableCollection<TranslationVM> Translations
        {
            get { return translations; }
            set { translations = value; OnPropertyChanged("Translations"); }
        }
        private TranslationVM _selectedItem;
        public TranslationVM SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }
        public EncodingVM EncodingVM { get; set; }
        #endregion

        // TODO refactor rm and ci
        public ModifyWordVM(List<Translation> translations, EncodingVM encodingVM, List<Language.Languages> languages, ResourceManager rm, CultureInfo ci)
        {
            EncodingVM = encodingVM;
            Translations = ConvertionHelper.ConvertTo(translations,languages);
            UILanguage1 = languages[0].ToDescription();
            UILanguage2 = languages[1].ToDescription();

            this.rm = rm;
            this.ci = ci;
        }

        #region Command
        private CommandHandlerWithParameter _selectionCommand;
        public CommandHandlerWithParameter SelectionCommand
        {
            get { return _selectionCommand ?? (_selectionCommand = new CommandHandlerWithParameter((item) => SelectionHandler((TranslationVM)item), true)); }

        }

        private CommandHandler _nextElementCommand;
        public CommandHandler NextElementCommand
        {
            get { return _nextElementCommand ?? (_nextElementCommand = new CommandHandler(() => NextElementHandler(), true)); }

        }

        private CommandHandler _previousElementCommand;
        public CommandHandler PreviousElementCommand
        {
            get { return _previousElementCommand ?? (_previousElementCommand = new CommandHandler(() => PreviousElementHandler(), true)); }

        }
        #endregion

        private void SelectionHandler(TranslationVM translation)
        {
            EncodingVM.SetItem(SelectedItem);
        }

        private void NextElementHandler()
        {
            SelectedItem = SelectedItem == null || SelectedItem == Translations.Last()
                ? Translations.First() 
                : Translations.Where(t => t.Id == SelectedItem.Id + 1).First();

            EncodingVM.SetItem(SelectedItem);
        }

        private void PreviousElementHandler()
        {
            SelectedItem = SelectedItem == null || SelectedItem == Translations.First()
               ? Translations.Last()
               : Translations.Where(t => t.Id == SelectedItem.Id - 1).First();

            EncodingVM.SetItem(SelectedItem);
        }
    }
}

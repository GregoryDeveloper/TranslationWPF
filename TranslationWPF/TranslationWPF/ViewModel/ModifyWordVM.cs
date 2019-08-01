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
using TranslationWPF.Exceptions;
using System.Windows;
using TranslationWPF.Services;

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
        private int index;

        // TODO refactor the reference tracking. Put an observalbe list in the mainwindow?
        //public List<Translation> TranslationsModel { get; set; }

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

        public TranslationService TranslationService { get; set; }
        #endregion

        // TODO refactor rm and ci
        public ModifyWordVM(TranslationService translationsService, EncodingVM encodingVM, List<Language.Languages> languages, ResourceManager rm, CultureInfo ci)
        {
            this.rm = rm;
            this.ci = ci;

            TranslationService = translationsService;

            if (TranslationService.Translations.Count == 0)
                throw new NoItemException(rm.GetString(StringConstant.noItemExceptionMessage, ci));

            EncodingVM = encodingVM;
            
            UILanguage1 = languages[0].ToDescription();
            UILanguage2 = languages[1].ToDescription();

            Translations = this.TranslationService.TranslationsVM;


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

        private CommandHandler _deleteCommand;
        public CommandHandler DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new CommandHandler(() => _deleteCommandHandler(), true)); }

        }

        private void _deleteCommandHandler()
        {
            TranslationService.RemoveTranslation(SelectedItem);
            //Translations.Remove(SelectedItem);
        }

        #endregion

        private void SelectionHandler(TranslationVM translation)
        {
            EncodingVM.SetItem(SelectedItem);
        }

        private void NextElementHandler()
        {

            SelectedItem = SelectedItem == null || SelectedItem == TranslationService.TranslationsVM.Last()
                ? TranslationService.TranslationsVM.First() 
                : TranslationService.TranslationsVM.SkipWhile( t => t != SelectedItem).Skip(1).FirstOrDefault();

            EncodingVM.SetItem(SelectedItem);
        }

        private void PreviousElementHandler()
        {
            SelectedItem = SelectedItem == null || SelectedItem == TranslationService.TranslationsVM.First()
               ? TranslationService.TranslationsVM.Last()
               : TranslationService.TranslationsVM.TakeWhile(t => t != SelectedItem).Last();

            EncodingVM.SetItem(SelectedItem);
        }
    }
}

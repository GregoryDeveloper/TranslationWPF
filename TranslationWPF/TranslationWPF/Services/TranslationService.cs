using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TranslationWPF.Model;
using TranslationWPF.ViewModel;

namespace TranslationWPF.Services
{
    public class TranslationService: INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public List<Translation> Translations { get; set; } = new List<Translation>();

        public TranslationService()
        { }

        public void AddTranslation(TranslationVM _translation)
        {
            // Add translations
            //Translation translation = new Translation();
            //translation.Languages[0] = _translation.Language1;
            //translation.Languages[1] = _translation.Language2;
            //translation.Id = _translation.Id;
            //translation.Languages[1].Type = _translation.Language2;
            //translation.Languages[1] = _translation.Language2;
            //translation.Languages[1] = _translation.Language2;


            Translations.Add(_translation.Translation);
            TranslationsVM.Add(_translation);
        }

        public void RemoveTranslation(TranslationVM translation)
        {
            // Remove translations
            Translations.Remove(translation.Translation);
            TranslationsVM.Remove(translation);

        }

        //private DatabaseService databaseService;
        //public DatabaseService DatabaseService
        //{
        //    get => this.databaseService;
        //    set
        //    {
        //        this.databaseService = value;
        //        this.Translations = databaseService.getTranslations;
        //    }
        //}

        private ObservableCollection<TranslationVM> translationsVM;

        public ObservableCollection<TranslationVM> TranslationsVM
        {
            get => this.translationsVM;
            set
            {
                this.translationsVM = value;
                OnPropertyChanged("Translations");
            }
        }
    }
}

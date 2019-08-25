using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private ObservableCollection<TranslationVM> translationsVM = new ObservableCollection<TranslationVM>();
        public ObservableCollection<TranslationVM> TranslationsVM
        {
            get => this.translationsVM;
            set
            {
                this.translationsVM = value;
                SetDisplayableTranslationsVM();
                OnPropertyChanged("TranslationsVM");
            }
        }

        private ObservableCollection<TranslationVM> displayableTranslationsVM  = new ObservableCollection<TranslationVM>();
        public ObservableCollection<TranslationVM> DisplayableTranslationsVM
        {
            get { return displayableTranslationsVM; }
            set { displayableTranslationsVM = value; OnPropertyChanged("DisplayableTranslationsVM"); }
        }


        private List<Language.Languages> languagesOrder = new List<Language.Languages>();
        public List<Language.Languages> LanguagesOrder
        {
            get { return languagesOrder; }
            set
            {
                languagesOrder = value;
                SetLanguagesOrder();
                SetDisplayableLanguages();
            }
        }

        public TranslationService()
        { }

        public void AddTranslation(TranslationVM _translation)
        {
            Translations.Add(_translation.Translation);
            TranslationsVM.Add(_translation);
        }

        public void AddTranslation(Translation _translation)
        {
            Translations.Add(_translation);
            translationsVM.Add(new TranslationVM(_translation));

        }

        public void RemoveTranslation(TranslationVM translation)
        {
            // Remove translations
            Translations.Remove(translation.Translation);
            TranslationsVM.Remove(translation);

        }

        public List<TranslationVM> GetTranslations(Language.Languages language)
        {
            return TranslationsVM.Where(t => t.Translation.HasLanguage(language)).ToList();
        }

        private void SetDisplayableTranslationsVM()
        {
            DisplayableTranslationsVM.Clear();
            foreach (var item in TranslationsVM)
            {
                if (item.Display == true)
                    DisplayableTranslationsVM.Add(item);
            }
        }

        private void SetDisplayableLanguages()
        {
            foreach (var item in TranslationsVM)
            {
                item.SetDisplay(LanguagesOrder);
            }

            SetDisplayableTranslationsVM();
        }

        private void SetLanguagesOrder()
        {
            foreach (var item in TranslationsVM)
            {
                item.AssignLanguages(LanguagesOrder);
            }
        }

        #region GetNextElement
        public TranslationVM GetNextOrFirstElement(TranslationVM translation)
        {
            if (translation == null)
            {
                return GetFirstElement();
            }
            else
            {
                return GetNextElement(translation);
            }
        }

        private TranslationVM GetNextElement(TranslationVM translation)
        {
            int i = GetNextElementIndex(translation);

            if (i < TranslationsVM.Count)
                return TranslationsVM[i];
            else
                return GetFirstElement();

        }

        private TranslationVM GetFirstElement()
        {

            int i = GetNextDisplayableElementIndex(0);

            return TranslationsVM[i];
        }
        private int GetNextElementIndex(TranslationVM translation)
        {
            int i = 0;

            while (i < TranslationsVM.Count && TranslationsVM[i] != translation)
            {
                i++;
            }

            i++;

            return GetNextDisplayableElementIndex(i);
        }

        private int GetNextDisplayableElementIndex(int index)
        {
            while (index < TranslationsVM.Count && TranslationsVM[index].Display == false)
            {
                index++;
            }

            return index;
        }
        #endregion

        #region GetPreviousElement
        public TranslationVM GetPreviousOrLastElement(TranslationVM translation)
        {
            if (translation == null)
            {
                return GetLastElement();
            }
            else
            {
                return GetPreviousElement(translation);
            }
        }

        private TranslationVM GetPreviousElement(TranslationVM translation)
        {
            int i = GetPreviousElementIndex(translation);

            if (i >= 0)
                return TranslationsVM[i];
            else
                return GetLastElement();

        }

        private TranslationVM GetLastElement()
        {

            int i = GetPreviousDisplayableElementIndex(TranslationsVM.Count - 1);

            return TranslationsVM[i];
        }
        private int GetPreviousElementIndex(TranslationVM translation)
        {
            int i = TranslationsVM.Count - 1;

            while (i > -1 && TranslationsVM[i] != translation)
            {
                i--;
            }

            i--;

            return GetPreviousDisplayableElementIndex(i);

        }

        private int GetPreviousDisplayableElementIndex(int index)
        {
            while (index > -1 && TranslationsVM[index].Display == false)
            {
                index--;
            }

            return index;
        }

        #endregion


    }
}

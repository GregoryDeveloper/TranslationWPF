using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        private bool isDirty = false;
        public bool IsDirty
        {
            get { return isDirty; }
        }


        public TranslationService()
        { }

        public void AddNewTranslationListToCurrentList(List<Translation> translations)
        {
            foreach (var item in translations)
            {
                AddTranslation(item);
            }

            isDirty = true;
        }

        /// <summary>
        /// Meant to be called when we import a new list hence isDirty = false
        /// </summary>
        /// <param name="translations"></param>
        public void CreateNewTranslationList(List<Translation> translations)
        {
            Clear();

            foreach (var item in translations)
            {
                AddTranslation(item);
            }

            isDirty = false;
        }

        public void Clear()
        {
            Translations.Clear();
            TranslationsVM.Clear();
            isDirty = true;

        }

        public void AddTranslation(TranslationVM _translation)
        {
            Translations.Add(_translation.Translation);
            TranslationsVM.Add(_translation);
            isDirty = true;
        }

        public void AddTranslation(Translation _translation)
        {
            Translations.Add(_translation);
            translationsVM.Add(new TranslationVM(_translation));
            isDirty = true;

        }

        public void RemoveTranslation(TranslationVM translation)
        {
 
            Translations.Remove(translation.Translation);
            TranslationsVM.Remove(translation);
            DisplayableTranslationsVM.Remove(translation);
            isDirty = true;

        }

        public List<TranslationVM> GetTranslations(Language.Languages language)
        {
            return TranslationsVM.Where(t => t.Translation.HasLanguage(language)).ToList();
        }

        public List<Language.Languages> GetLanguages()
        {
            List<Language.Languages> languages = new List<Language.Languages>();

            foreach (var translation in Translations)
            {
                AddLanguagesIfNotExist(languages,translation);
            }

            return languages;
        }

        private void AddLanguagesIfNotExist(List<Language.Languages> languages,Translation translation)
        {
            var translationLanguages = translation.GetCurrentLanguages();

            foreach (var language in translationLanguages)
            {
                AddIfNotExist(languages, language);
            }
        }

        private void AddIfNotExist(List<Language.Languages> languages, Language.Languages language)
        {
            if (!languages.Any(l => l == language))
            {
                languages.Add(language);
                isDirty = true;
            }
        }

        private void SetDisplayableTranslationsVM()
        {
            DisplayableTranslationsVM.Clear();
            foreach (var item in TranslationsVM)
            {
                if (!item.Display)
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
                item.SaveLanguagesInOrder(LanguagesOrder);
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
            while (index < TranslationsVM.Count && !TranslationsVM[index].Display)
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
            while (index > -1 && !TranslationsVM[index].Display)
            {
                index--;
            }

            return index;
        }

        #endregion

        public void Save(string filename)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(Translations));
            isDirty = false;
        }

        public List<Translation> FormattedLoad(string filename)
        {
            string content = File.ReadAllText(filename);
            var translations =  JsonConvert.DeserializeObject<List<Translation>>(content);

            isDirty = false;

            return translations;
        }


    }
}

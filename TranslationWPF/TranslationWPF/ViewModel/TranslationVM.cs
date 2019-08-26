using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslationWPF.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace TranslationWPF.ViewModel
{
    // TODO: refacto properties must be bind to the translation object properties (reference)
    public class TranslationVM : INotifyPropertyChanged
    {
        #region OnpropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  

        #region Properies
        // TODO: refacto useless now check
        public int Id { get; }

        public Translation Translation { get; set; }

        private Language language1;
        public Language Language1
        {
            get { return language1; }
            set { language1 = value; OnPropertyChanged("Language1"); }
        }

        private Language language2;
        public Language Language2
        {
            get { return language2; }
            set { language2 = value; OnPropertyChanged("Language2"); }
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

        public Language.Types[] WordTypes
        {
            get { return Language1.GetTypesAvailables(); }
        }
        public Language.Types[] TranslationTypes
        {
            get { return Language2.GetTypesAvailables(); }
        }


        public ObservableCollection<string> Language1Synonyms { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Language2Synonyms { get; set; } = new ObservableCollection<string>();


        private string _synonyms1String;
        public string Synonyms1String
        {
            get
            {
                if ((_synonyms1String == null || _synonyms1String == "") && Language1.Synonysms.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language1.Synonysms)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }
                    sb.Remove(sb.Length - 1, 1);
                    _synonyms1String = sb.ToString();
                }
                return _synonyms1String;
            }
        }

        private string _synonyms2String;
        public string Synonyms2String
        {
            get
            {
                if ((_synonyms2String == null || _synonyms2String == "") && Language2.Synonysms.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language2.Synonysms)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }
                    sb.Remove(sb.Length - 1, 1);
                    _synonyms2String = sb.ToString();
                }
                return _synonyms2String;
            }
        }

        private string line;
        public string Line
        {
            get { return line; }
            set { line = value; OnPropertyChanged("Line"); }
        }

        public bool Display { get; set; } = true;

        #endregion

        public TranslationVM() { }

        public TranslationVM(TranslationVM translationVM)
        {
            Translation = translationVM.Translation;

            Language1 = translationVM.Language1;
            Language2 = translationVM.Language2;

            WordSelectedType = translationVM.WordSelectedType;
            TranslationSelectedType = translationVM.TranslationSelectedType;

            Language1Synonyms = new ObservableCollection<string>(translationVM.Language1Synonyms);
            Language2Synonyms = new ObservableCollection<string> (translationVM.Language2Synonyms);

        }

        public TranslationVM(Translation translation, List<Language.Languages> languages)
        {
            Id = translation.Id;

            AssignLanguages(translation, languages);

            Language1.Synonysms.ForEach(s => Language1Synonyms.Add(s));
            Language2.Synonysms.ForEach(s => Language2Synonyms.Add(s));

            Translation = translation;

            Line = translation.Line;

        }

        public TranslationVM(Translation translation)
        {
            Translation = translation;

            Id = translation.Id;

            List<Language.Languages> languages = SetDefaultLanguagesOrder(translation);
            AssignLanguages(languages);

            Line = translation.Line;

        }

        public void SetDisplay(List<Language.Languages> languages)
        {

            foreach (var language in languages)
            {
                if (!Translation.HasLanguage(language))
                {
                    Display = false;
                    return;
                }
            }
            Display = true;

        }


        public void Save()
        {
            Translation.Languages[0] = Language1;
            Translation.Languages[1] = Language2;


            Translation.SetLanguageType(Language1.GetLanguage(), WordSelectedType);
            Translation.SetLanguageType(Language2.GetLanguage(), TranslationSelectedType);

            Translation.SetLanguageSynonysms(Language1.GetLanguage(), Language1Synonyms.ToList());
            Translation.SetLanguageSynonysms(Language2.GetLanguage(), Language2Synonyms.ToList());
            

        }

        //TODO refactoring
        private void AssignLanguages(Translation translation, List<Language.Languages> languages)
        {
            try
            {
                Language1 = GetLanguage(translation.Languages, languages[0]);
                Language2 = GetLanguage(translation.Languages, languages[1]);
            }
            catch(KeyNotFoundException ex)
            {
                Language1 = translation.Languages[0];
                Language2 = translation.Languages[1];
            }

        }

        //TODO refactoring
        public void AssignLanguages(List<Language.Languages> languages)
        {
            try
            {
                Language1 = GetLanguage(Translation.Languages, languages[0]);
                Language2 = GetLanguage(Translation.Languages, languages[1]);
            }

            catch (KeyNotFoundException ex)
            {
                Language1 = Translation.Languages[0];
                Language2 = Translation.Languages[1];
            }

        }



        /// <summary>
        /// Returns the language from the model to attatch to the viewmodel
        /// </summary>
        /// <param name="languages"> list of languages available</param>
        /// <param name="languageType">language meant to be assigned (eg spanish)</param>
        private Language GetLanguage(List<Language> languages, Language.Languages languageType )
        {
            foreach (var language in languages)
            {
                if (languageType == language.GetLanguage()) 
                    return language;

            }

            throw new KeyNotFoundException($"The language {languageType} hasn't been found");
        }

       

        private void AssignWord(Language language)
        {
            Language1 = language;
            WordSelectedType = language.Type;
            Language1Synonyms.Clear();
            Language1.Synonysms.ForEach(s => Language1Synonyms.Add(s));
        }

        private void AssignTranslation(Language language)
        {
            Language2 = language;
            TranslationSelectedType = language.Type;
            Language2Synonyms.Clear();
            Language2.Synonysms.ForEach(s => Language2Synonyms.Add(s));

        }

        private List<Language.Languages> SetDefaultLanguagesOrder(Translation translation)
        {
            List<Language.Languages> languages = new List<Language.Languages>();
            languages.Add(translation.Languages[0].GetLanguage());
            languages.Add(translation.Languages[1].GetLanguage());

            return languages;
        }


    }



}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TranslationWPF.Helper;

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

        public Translation Translation { get; set; } = new Translation();

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


        //public TranslationTrainingVM Training { get; set; } = new TranslationTrainingVM();
        #endregion

        private TranslationVM() { }

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

            Translation.Languages.Add(Language1);
            Translation.Languages.Add(Language2);

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


        public void Save()
        {

            Translation.Languages[0].Type = WordSelectedType;
            Translation.Languages[1].Type = TranslationSelectedType;

            Translation.Languages[0].Synonysms = Language1Synonyms.ToList();
            Translation.Languages[1].Synonysms = Language2Synonyms.ToList();

        }

        //TODO refactoring
        private void AssignLanguages(Translation translation, List<Language.Languages> languages)
        {
            if (languages[0] == translation.Languages[0].GetLanguage())
            {
                Language1 = translation.Languages[0];
                Language2 = translation.Languages[1];
            }
            else
            {
                Language1 = translation.Languages[1];
                Language2 = translation.Languages[0];
            }
        }

        //TODO refactoring
        public void AssignLanguages(List<Language.Languages> languages)
        {
            if (languages[0] == Translation.Languages[0].GetLanguage())
            {
                AssignWord(Translation.Languages[0]);
                AssignTranslation(Translation.Languages[1]);
            }
            else
            {
                AssignWord(Translation.Languages[1]);
                AssignTranslation(Translation.Languages[0]);

            }
           
        }

        private void AssignWord(Language language)
        {
            Language1 = language;
            WordSelectedType = language.Type;
            Language1Synonyms.Clear();
            Language1.Synonysms.ForEach(s => Language1Synonyms.Add(s));
            //Translation.Languages.Add(Language1);
        }

        private void AssignTranslation(Language language)
        {
            Language2 = language;
            TranslationSelectedType = language.Type;
            Language2Synonyms.Clear();
            Language2.Synonysms.ForEach(s => Language2Synonyms.Add(s));
            //Translation.Languages.Add(Language2);

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


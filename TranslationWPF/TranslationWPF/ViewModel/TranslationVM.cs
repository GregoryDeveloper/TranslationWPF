using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TranslationWPF.ViewModel
{
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
        #endregion

        private TranslationVM() { }
        public TranslationVM(Translation translation)
        {
            Id = translation.Id;

            Language1 = translation.Languages[0];
            Language2 = translation.Languages[1];

            Language1.Synonysms.ForEach(s => Language1Synonyms.Add(s));
            Language2.Synonysms.ForEach(s => Language2Synonyms.Add(s));

            Translation.Languages.Add(Language1);
            Translation.Languages.Add(Language2);

        }

        public void Save()
        {

            Translation.Languages[0].Type = WordSelectedType;
            Translation.Languages[1].Type = TranslationSelectedType;

            Translation.Languages[0].Synonysms = Language1Synonyms.ToList();
            Translation.Languages[1].Synonysms = Language2Synonyms.ToList();

        }

    }
}

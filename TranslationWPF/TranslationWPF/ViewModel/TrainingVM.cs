using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class TrainingVM : INotifyPropertyChanged
    {
        #region Properties

        private Translation Translation { get; set; }
        private Language.Languages languageTrained;
        private Language.Languages referenceLanguage;

        public Language Language1 { get; set; }
        public Language Language2 { get;set; }


        public string Input { get; set; } = "";

        private bool? hasMistake = null;

        public bool? HasMistake
        {
            get { return hasMistake; }
            set { hasMistake = value; OnPropertyChanged("HasMistake"); }
        }
        
        private int mistakeCount = 0;
        public int MistakesCount
        {
            get { return mistakeCount; }
            set
            {
                mistakeCount = value;
                if (MistakesCount != 0)
                    HasMistake = true;
                else
                    HasMistake = null;
            }
        }
        public bool HasTried { get; set; } = false;
        private bool found;
        public bool Found
        {
            get { return found; }
            set { found = value; OnPropertyChanged("Found"); }
        }

        #endregion

        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TrainingVM(Translation translation, Language.Languages referenceLanguage, Language.Languages languageTrained)
        {
            this.Translation = translation;
            this.languageTrained = languageTrained;
            this.referenceLanguage = referenceLanguage;

            Language1 = Translation.GetLanguage(referenceLanguage);
            Language2 = Translation.GetLanguage(languageTrained);

        }

        public void Refresh()
        {
            Input = "";
            HasMistake = null;
            MistakesCount = 0;
            HasTried = false;
            found = false;
        }

        public string GetRightValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Language2.Value);
            Language2.Synonysms.ForEach(item =>
            {
                sb.Append("/")
                    .Append(item);
            });
            return sb.ToString();
        }
    }
}

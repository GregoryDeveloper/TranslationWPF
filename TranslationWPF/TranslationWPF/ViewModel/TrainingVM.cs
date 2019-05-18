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
        private Translation Translation { get; set; }
        private Language.Languages languageTrained;

        public int Id
        {
            get {return Translation.Id; }
        }
        public Language Language1
        {
            get
            {
                foreach (var item in Translation.Languages)
                {
                    if (item.GetLanguage() == languageTrained)
                        return item;
                }
                return Translation.Languages[0];
            }
        }
        public Language Language2
        {
            get
            {
                foreach (var item in Translation.Languages)
                {
                    if (item.GetLanguage() != languageTrained)
                        return item;
                }
                return Translation.Languages[0];
            }
        }

        public string Input { get; set; } = "";
        public int MistakesCount { get; set; } = 0;
        public bool HasTried { get; set; } = false;
        private bool? found;
        public bool? Found
        {
            get { return found; }
            set { found = value; OnPropertyChanged("Found"); }
        }

        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TrainingVM(Translation translation, Language.Languages languageTrained)
        {
            this.Translation = translation;
            this.languageTrained = languageTrained;
        }

        public void Refresh()
        {
            Input = "";
            MistakesCount = 0;
            HasTried = false;
            found = null;
        }

        public string GetRightValues()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Language1.Value);
            Language1.Synonysms.ForEach(item =>
            {
                sb.Append("/")
                    .Append(item);
            });
            return sb.ToString();
        }
    }
}

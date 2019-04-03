using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TranslationWPF.ViewModel
{
    public class TranslationTrainingVM:INotifyPropertyChanged
    {
        #region Propertychanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public string Input { get; set; }
        public int MistakesCount { get; set; } = 0;
        public bool HasTried { get; set; } = false;
        private bool? foundCount;
        public bool? Found
        {
            get { return foundCount; }
            set { foundCount = value; OnPropertyChanged("Found"); }
        }

        public TranslationTrainingVM()
        {

        }
    }



}


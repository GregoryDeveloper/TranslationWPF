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
        private Translation translation { get; set; }

        public int Id
        {
            get {return translation.Id; }
        }
        public Language Language1
        {
            get { return translation.Languages[0]; }
        }
        public Language Language2
        {
            get{ return translation.Languages[1]; }
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

        public TrainingVM(Translation translation)
        {
            this.translation = translation;
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

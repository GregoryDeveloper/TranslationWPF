using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;
using System.Collections.ObjectModel;
using TranslationWPF.Helper;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TranslationWPF.ViewModel
{
    public class ModifyWordVM: INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Properties
        private ObservableCollection<TranslationVM> translations;
        public ObservableCollection<TranslationVM> Translations
        {
            get { return translations; }
            set { translations = value; OnPropertyChanged("Translations"); }
        }

        #endregion
        public ModifyWordVM(List<Translation> translations)
        {
            this.translations = ConvertionHelper.ConvertTo(translations);
        }
    }
}

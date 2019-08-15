﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class AddTranslationVM: INotifyPropertyChanged
    {

        #region OnpropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  

        public Translation Translation { get; set; }

        private string languageValue;

        public string LanguageValue
        {
            get { return languageValue; }
            set { languageValue = value; OnPropertyChanged("LanguageValue"); }
        }

        public AddTranslationVM(Translation translation,Language.Languages language)
        {
            Translation = translation;
            LanguageValue = Translation.GetLanguageValue(language);
        }
    }
}

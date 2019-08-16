﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                OnPropertyChanged("TranslationsVM");
            }
        }

        private List<Language.Languages> languagesOrder = new List<Language.Languages>();
        public List<Language.Languages> LanguagesOrder
        {
            get { return languagesOrder; }
            set
            {
                languagesOrder = value;

                foreach (TranslationVM item in TranslationsVM)
                {
                    item.AssignLanguages(LanguagesOrder);
                }
                SetDisplayableLanguages();
                SetVisibleLanguages();
            }
        }

        public TranslationService()
        { }

        public void AddTranslation(TranslationVM _translation)
        {
            Translations.Add(_translation.Translation);
            TranslationsVM.Add(_translation);
        }

        public void AddTranslation(Translation _translation)
        {
            Translations.Add(_translation);
            translationsVM.Add(new TranslationVM(_translation));


        }

        public void RemoveTranslation(TranslationVM translation)
        {
            // Remove translations
            Translations.Remove(translation.Translation);
            TranslationsVM.Remove(translation);

        }

        public List<Translation> GetTranslations(Language.Languages language)
        {
            return Translations.Where(t => t.HasLanguage(language)).ToList();
        }

        private void SetDisplayableLanguages()
        {
            foreach (var item in TranslationsVM)
            {
                item.SetDisplay(LanguagesOrder);
            }   
        }

        private void SetVisibleLanguages()
        {
            foreach (var item in TranslationsVM)
            {
                item.AssignLanguages(LanguagesOrder);
            }
        }

        
    }
}

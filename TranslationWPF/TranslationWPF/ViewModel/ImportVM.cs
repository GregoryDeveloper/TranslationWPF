using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using TranslationWPF.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TranslationWPF.Helper;
using Newtonsoft.Json;
using System.Resources;
using System.Globalization;
using TranslationWPF.Languages;
using TranslationWPF.Services;
using System.Windows;
using TranslationWPF.Model.TranslationBuilder;
using TranslationWPF.Commands;

namespace TranslationWPF.ViewModel
{
    public class ImportVM:INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        enum Import
        {
            Formatted,
            Unformatted
        }

        #region Properties

        #region UI Properties

        private string importUI;
        public string ImportUI { get { return importUI = importUI ?? rm.GetString(StringConstant.import, ci); } }

        private string formattedImportUI;
        public string FormattedImportUI { get { return formattedImportUI = formattedImportUI ?? rm.GetString(StringConstant.formattedImport, ci); } }

        private string exportUI;
        public string ExportUI { get { return exportUI = exportUI ?? rm.GetString(StringConstant.export, ci); } }

        private string textExportUI;
        public string TextExportUI { get { return textExportUI = textExportUI ?? rm.GetString(StringConstant.textExportUI, ci); } }

        
        private string language1;
        public string Language1 { get { return language1 = language1 ?? rm.GetString(languagesOrder[0].ToDescription(), ci); } }

        private string languageComment1;
        public string LanguageComment1 { get { return languageComment1 = languageComment1 ?? rm.GetString(StringConstant.comment, ci);  } }

        private string languageExemple1;
        public string LanguageExemple1 { get { return languageExemple1 = languageExemple1 ?? rm.GetString(StringConstant.exemple, ci); } }

        private string languageSynonym1;
        public string LanguageSynonym1 { get { return languageSynonym1 = languageSynonym1 ?? rm.GetString(StringConstant.synonysms, ci); } }

        private string languageType1;
        public string LanguageType1 { get { return languageType1 = languageType1 ?? rm.GetString(StringConstant.type, ci); } }

        private string language2;
        public string Language2 { get { return language2 = language2 ?? rm.GetString(languagesOrder[1].ToDescription(), ci); } }

        private string line;
        public string Line { get { return line = line ?? rm.GetString(StringConstant.line, ci); } }

        #endregion

        private ObservableCollection<TranslationVM> translations;
        public ObservableCollection<TranslationVM> Translations
        {
            get { return translations; }
            set { translations = value; OnPropertyChanged("Translations");}
        }

        TranslationService translationService;
        List<Language.Languages> languagesOrder;

        #endregion
        ResourceManager rm;
        readonly CultureInfo ci;

        public ImportVM(List<Language.Languages> languages)
        {
            this.rm = LanguageSingleton.Instance.ResourceManager;
            this.ci = LanguageSingleton.Instance.CultureInfo;
            this.translationService = ResourceHelper.GetResource<TranslationService>(Constants.TRANSLATION_SERVICE);

            languagesOrder = languages;

            Translations = translationService.TranslationsVM;

        }
        #region Commands
        private CommandHandler _importCommand;
        public CommandHandler ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new CommandHandler(() => ExecuteImport(), true)); }

        }

        private CommandHandler _formattedImportCommand;
        public CommandHandler FormattedImportCommand
        {
            get { return _formattedImportCommand ?? (_formattedImportCommand = new CommandHandler(() => ExecuteFormattedImport(), true)); }

        }

        private CommandHandler _exportCommand;
        public CommandHandler ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new CommandHandler(() => ExecuteFormattedExport(), true)); }

        }

        private CommandHandler _textExportCommand;
        public CommandHandler TextExportCommand
        {
            get { return _textExportCommand ?? (_textExportCommand = new CommandHandler(() => ExecuteTextExport(), true)); }

        }

        
        #endregion
        #region Methods
        private void ExecuteImport()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                List<Translation> importedTranslations =  GetWordsFromImport(ofd.FileName, Import.Unformatted);
                translationService.CreateNewTranslationList(importedTranslations);
            }

        }

        private void ClearTranslations()
        {
            Translations.Clear();
        }

        private void ExecuteFormattedImport()
        {
            var message = rm.GetString(StringConstant.keepList, ci);
            var caption = rm.GetString(StringConstant.keepListCaption, ci);
            MessageBoxResult result =  MessageBox.Show(message, caption,MessageBoxButton.YesNo);

            OpenFileDialog ofd = new OpenFileDialog();
            List<Translation> translations;

            if (ofd.ShowDialog() == true)
            {
                translations = translationService.FormattedLoad(ofd.FileName);

                if (result == MessageBoxResult.Yes)             
                    translationService.AddNewTranslationListToCurrentList(translations);
                
                else
                    translationService.CreateNewTranslationList(translations);
            }
        }

        private List<Translation> GetWordsFromImport(string path, Import import)
        {
            List<Translation> translations = new List<Translation>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {

                        String line = sr.ReadLine();

                        TranslationDirector director = new TranslationDirector();
                        TranslationBuilder translationBuilder;
                        switch (import)
                        {
                            case Import.Formatted:
                                 translationBuilder = new TranslationFormattedBuilder(line);
                                break;
                            case Import.Unformatted:
                                 translationBuilder = new TranslationUnformattedBuilder(line);
                                break;
                            default:
                                throw new InvalidDataException();

                        }


                        director.Construct(translationBuilder);

                        translations.Add(translationBuilder.GetResult());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read the file");
            }
            return translations;
        }

        private void ExecuteFormattedExport()
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            if (sfd.ShowDialog() == true)
            {
                translationService.Save(sfd.FileName);
            }

        }

        private void ExecuteTextExport()
        {
            translationService.LanguagesOrder = PickupLanguageHelper.PickUpLanguages(translationService, rm, ci);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            if (sfd.ShowDialog() == true)
            {
                SaveTranslationToTextFile(sfd.FileName);
            }

           
        }

        private void SaveTranslationToTextFile(string filename)
        {
            using (StreamWriter file =
           new StreamWriter(filename))
            {
                foreach (Translation translation in translationService.Translations)
                {
                    file.WriteLine(translation.ToString());
                }
            }
        }


        #endregion
    }
}

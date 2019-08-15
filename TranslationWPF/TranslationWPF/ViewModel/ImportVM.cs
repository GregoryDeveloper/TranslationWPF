using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public List<Translation> TranslationModel { get; set; }

        #endregion
        ResourceManager rm;
        CultureInfo ci;
        private List<Language.Languages> languagesOrder;

        public ImportVM(List<Translation> translations, List<Language.Languages> languages,ResourceManager rm, CultureInfo ci)
        {
            this.rm = rm;
            this.ci = ci;
            languagesOrder = languages;
            Translations = ConvertionHelper.ConvertTo(translations,languages);
            TranslationModel = translations;
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
        #endregion
        #region Methods
        private void ExecuteImport()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                TranslationModel = GetWordsFromImport(ofd.FileName, Import.Unformatted);
                ClearTranslations();
                Translations = ConvertionHelper.ConvertTo(TranslationModel,languagesOrder);
            }

        }

        private void ClearTranslations()
        {
            Translations.Clear();
        }

        private void ExecuteFormattedImport()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<Translation> translations;
            if (ofd.ShowDialog() == true)
            {
                string content = File.ReadAllText(ofd.FileName);
                translations = JsonConvert.DeserializeObject<List<Translation>>(content);
                Translations = ConvertionHelper.ConvertTo(translations, languagesOrder);
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
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                sfd.FilterIndex = 1;

                if (sfd.ShowDialog() == true)
                {
                    List<Translation> translations = new List<Translation>();
                    foreach (TranslationVM item in Translations)
                    {
                        translations.Add(item.Translation);
                    }
                    File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(translations));
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        #endregion
    }
}

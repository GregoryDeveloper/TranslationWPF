using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class ImportVM
    {
        enum Import
        {
            Formatted,
            Unformatted
        }

        public ObservableCollection<TranslationVM> Translations { get; set; } = new ObservableCollection<TranslationVM>();

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
        
        private void ExecuteImport()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<Translation> translations;
            if (ofd.ShowDialog() == true)
            {
                translations = GetWordsFromImport(ofd.FileName, Import.Unformatted);
                ClearTranslations();
                foreach (Translation t in translations)
                {
                    Translations.Add(new TranslationVM()
                    {
                        Translation = t,
                        Language1 = t.Languages[0],
                        Language2 = t.Languages[1],
                    });
                }
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
                translations = GetWordsFromImport(ofd.FileName, Import.Formatted);
                ClearTranslations();
                foreach (Translation t in translations)
                {
                    Translations.Add(new TranslationVM()
                    {
                        Translation = t,
                        Language1 = t.Languages[0],
                        Language2 = t.Languages[1],
                    });
                }
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
                        Translation translation = new Translation();

                        String line = sr.ReadLine();
                        translation.Line = line;

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
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 1;

                if (sfd.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (TranslationVM t in Translations)
                        {
                            sw.WriteLine(t.Translation.GetTranslationStringRepresentation());
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}

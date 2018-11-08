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
        public ObservableCollection<TranslationVM> Translations { get; set; } = new ObservableCollection<TranslationVM>();

        private CommandHandler _importCommand;
        public CommandHandler ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new CommandHandler(() => ExecuteImport(), true)); }

        }

        private CommandHandler _exportCommand;
        public CommandHandler ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new CommandHandler(() => ExecuteImport(), true)); }

        }

        private void ExecuteImport()
        {

            OpenFileDialog ofd = new OpenFileDialog();
            List<Translation> translations;
            if (ofd.ShowDialog() == true)
            {
                translations = GetWordsFromImport(ofd.FileName);
                foreach (Translation t in translations)
                {
                    Translations.Add(new TranslationVM()
                    {
                        Language1 = t.Translations[0].Value,
                        Language1Comment = t.Translations[0].Comment,
                        Language2 = t.Translations[1].Value,
                        Language2Comment = t.Translations[1].Comment,
                        Line = t.Line,
                        Language1Example = t.Translations[0].Example,
                        Language2Example = t.Translations[1].Example,
                        Language1Synonyms = t.Translations[0].Synonysms,
                        Language2Synonyms = t.Translations[1].Synonysms,
                        Language1Type = t.Translations[0].Type,
                        Language2Type = t.Translations[1].Type
                    });
                }
            }

        }

        private List<Translation> GetWordsFromImport(string path)
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
                        TranslationBuilder translationBuilder = new TranslationUnformattedBuilder(line);

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

    }
}

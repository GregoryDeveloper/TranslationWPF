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
            get { return _exportCommand ?? (_exportCommand = new CommandHandler(() => ExecuteFormattedExport(), true)); }

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
                        Translation = t,
                        Language1 = t.Languages[0],
                        Language2 = t.Languages[1],
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

        private void ExecuteFormattedExport()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 2;

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

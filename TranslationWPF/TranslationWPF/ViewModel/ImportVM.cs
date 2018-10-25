using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using TranslationLibrary;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class ImportVM
    {
        public ObservableCollection<TranslationVM> Words { get; set; } = new ObservableCollection<TranslationVM>();

        private  CommandHandler _importCommand;
            
        public CommandHandler ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new CommandHandler(() => ExecuteImportAsync(), true)); }
            
        }

        private void ExecuteImportAsync()
        {

            OpenFileDialog ofd = new OpenFileDialog();
            List<Translation> translations;
            if (ofd.ShowDialog() == true)
            {
                translations = GetWordsFromImport(ofd.FileName);
                foreach (Translation t in translations)
                {
                    Words.Add(new TranslationVM() { Language1 = t.Translations[0].Value,Language1Comment= t.Translations[0].Comment, Language2 = t.Translations[1].Value,
                        Language2Comment = t.Translations[1].Comment, Line = t.Line,Language1Example = t.Translations[0].Example,
                        Language2Example = t.Translations[1].Example, Language1Synonyms = t.Translations[0].Synonysms,
                        Language2Synonyms = t.Translations[1].Synonysms
                    });
                }
            }
            
        }

        private List<Translation> GetWordsFromImport(string path)
        {
            List<Translation> translations = new List<Translation >();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        ImportSingleton importSingleton = ImportSingleton.Instance;
                        Translation translation = new Translation();
                        English englishWord = new English();
                        French frenchWord = new French();

                        translation.Translations.Add(frenchWord);
                        translation.Translations.Add(englishWord);


                        List<string> synonyms;
                        string wordLine = "";
                        string translationLine = "";
                        String line = sr.ReadLine();
                        line = importSingleton.RemoveTabs(line);
                        translation.Line = line;
                        (wordLine, translationLine) = importSingleton.SplitLine(line);
                        (frenchWord.Comment, wordLine) = importSingleton.ExtractComment(wordLine);
                        (englishWord.Comment, translationLine) = importSingleton.ExtractComment(translationLine);
                        (frenchWord.Example, frenchWord.Value) = importSingleton.ExtractExample(wordLine);
                        (englishWord.Example, englishWord.Value) = importSingleton.ExtractExample(translationLine);
                        (synonyms, frenchWord.Value) = importSingleton.ExtractSynonyms(frenchWord.Value);
                        frenchWord.Synonysms.AddRange(synonyms);
                        (synonyms, englishWord.Value) = importSingleton.ExtractSynonyms(englishWord.Value);
                        englishWord.Synonysms.AddRange(synonyms);
                        //word = GetWord(line);
                        //translation = GetTranslation(line);
                        translations.Add(translation);
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

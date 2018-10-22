using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using TranslationLibrary;

namespace TranslationWPF.ViewModel
{
    public class ImportVM
    {
        public ObservableCollection<WordVM> Words { get; set; } = new ObservableCollection<WordVM>();

        private  CommandHandler _importCommand;
            
        public CommandHandler ImportCommand
        {
            get { return _importCommand ?? (_importCommand = new CommandHandler(() => ExecuteImportAsync(), true)); }
            
        }

        private void ExecuteImportAsync()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<Word> words;
            if (ofd.ShowDialog() == true)
            {
                words = GetWordsFromImport(ofd.FileName);
                foreach (Word w in words)
                {
                    Words.Add(new WordVM() { Language1 = w.BasicWord,Language1Comment= w.FrenchComment, Language2 = w.Translation,Language2Comment = w.EnglishComment, Line = w.Line,
                        Language1Example = w.WordExample,Language2Example = w.TranslationExample, Language1Synonyms = w.BasicWordSynonyms,Language2Synonyms = w.TranslationSynonyms });
                }
            }
            
        }

        private List<Word> GetWordsFromImport(string path)
        {
            List<Word> words = new List<Word>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        ImportSingleton importSingleton = ImportSingleton.Instance;
                        Word word = new Word();
                        List<string> synonyms;
                        string wordLine = "";
                        string translationLine = "";
                        String line = sr.ReadLine();
                        line = importSingleton.RemoveTabs(line);
                        word.Line = line;
                        (wordLine, translationLine) = importSingleton.SplitLine(line);
                        (word.FrenchComment, wordLine) = importSingleton.ExtractComment(wordLine);
                        (word.EnglishComment, translationLine) = importSingleton.ExtractComment(translationLine);
                        (word.WordExample, word.BasicWord) = importSingleton.ExtractExample(wordLine);
                        (word.TranslationExample, word.Translation) = importSingleton.ExtractExample(translationLine);
                        (synonyms, word.BasicWord) = importSingleton.ExtractSynonyms(word.BasicWord);
                        word.BasicWordSynonyms.AddRange(synonyms);
                        (synonyms, word.Translation) = importSingleton.ExtractSynonyms(word.Translation);
                        word.TranslationSynonyms.AddRange(synonyms);
                        //word = GetWord(line);
                        //translation = GetTranslation(line);
                        words.Add(word);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read the file");
            }
            return words;
        }


        
    }
}

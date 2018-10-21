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

        private  void ExecuteImportAsync()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            List<Word> words;
            if (ofd.ShowDialog() == true)
            {
                ImportSingleton importSingleton = ImportSingleton.Instance;
                words = importSingleton.ReadFile(ofd.FileName);
                foreach (Word w in words)
                {
                    Words.Add(new WordVM() { Language1 = w.BasicWord,Language1Comment= w.FrenchComment, Language2 = w.Translation,Language2Comment = w.EnglishComment, Line = w.Line,
                        Language1Example = w.WordExample,Language2Example = w.TranslationExample, Language1Synonyms = w.BasicWordSynonyms,Language2Synonyms = w.TranslationSynonyms });
                }
            }
            
        }


        
    }
}

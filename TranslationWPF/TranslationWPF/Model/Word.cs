using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationLibrary
{
    public class Word
    {
        public string BasicWord { get; set; }
        public string Translation { get; set; }


        public string FrenchComment { get; set; }
        public string EnglishComment { get; set; }

        public string WordExample { get; set; }
        public string TranslationExample { get; set; }

        public List<string> BasicWordSynonyms { get; set; } = new List<string>();
        public List<string> TranslationSynonyms { get; set; } = new List<string>();


        public string Line { get; set; }

        public bool Separable { get; set; }

        public Word()
        {

        }

        public Word(string word, string translation,string line)
        {
            BasicWord = word;
            Translation = translation;
            Line = line;
        }

        


    }
}

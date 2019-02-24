using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public abstract class LanguageBuilder
    {
        protected string modifiedLine;
        protected Language language;

        /// <summary>
        /// Extract the comment from the unformatted file imported
        /// </summary>
        public void CommenUnformattedExtraction()
        {
            string line = "";

            if (!modifiedLine.Contains("(") || !modifiedLine.Contains(")") || modifiedLine.Contains('='))
            {
                language.Comment = "";
                return;
            }

            line = ExtractUntilCaractere(modifiedLine, 0, '(');
            language.Comment = ExtractUntilCaractere(modifiedLine, modifiedLine.IndexOf('(') + 1, ')');
            line += ExtractUntilCaractere(modifiedLine, modifiedLine.IndexOf(')') + 1);
            modifiedLine = line;
        }
        public void ExampleUnformattedExtraction()
        {
            string[] lines;

            if (!modifiedLine.Contains("ex:") && !modifiedLine.Contains("example:") && !modifiedLine.Contains("exemple:"))
            {
                language.Example = "";
                return;
            }

            lines = modifiedLine.Split(':');

            if (!CheckIfExample(lines[0]))
            {
                language.Example = "";
                return;
            }
            modifiedLine = lines[0].Remove(lines[0].Length - 2);
            language.Example = CleanWhiteSpaces(lines[1]);

        }
        public void SynonymsUnformattedExtraction()
        {
            string wordLine = modifiedLine;
            List<string> synonyms = new List<string>();
            if (wordLine.Contains("(") || wordLine.Contains(")") || wordLine.Contains('='))
            {
                wordLine = ExtractUntilCaractere(modifiedLine, 0, '(');
                synonyms.Add(ExtractUntilCaractere(modifiedLine, modifiedLine.IndexOf('=') + 1, ')'));
            }

            if (wordLine.Contains(','))
            {
                string[] words = wordLine.Split(',');

                words = ExtractFirstCharIfWhiteSpace(words);
                wordLine = words[0];
                synonyms.AddRange(words.Skip(1));
            }
            language.Value = wordLine;
            language.Synonysms = synonyms;
        }
        public void ProceedGetType()
        {
            language.Type = language.GetType().ToString();
        }
        public void FormattedExtraction()
        {
            language.FillFromStringRepresenttion(modifiedLine);
        }

        public abstract Language GetResult();

        private string ExtractUntilCaractere(string line, int index, char? caractere = null)
        {
            StringBuilder sb = new StringBuilder();

            while (index < line.Length && line[index] != caractere)
            {
                sb.Append(line[index]);
                index++;
            }
            return sb.ToString();
        }
        private bool CheckIfExample(string line)
        {
            if (line[line.Length - 2] == 'e' && line[line.Length - 1] == 'x')
                return true;
            if (line.Contains("example") || line.Contains("exemple"))
                return true;
            return false;
        }

        private static string[] ExtractFirstCharIfWhiteSpace(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = CleanWhiteSpaces(array[i]);
            }
            return array;
        }
        private static string CleanWhiteSpaces(string s)
        {
            if (Char.IsWhiteSpace(s[0]))
                s =  s.Remove(0, 1);
            if (Char.IsWhiteSpace(s[s.Length-1]))
                s = s.Remove(s.Length - 1, 1);

            return s;
        }
    }
}

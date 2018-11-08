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

        public void ExtractComment()
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
        public void ExtractExample()
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
            language.Example = lines[1];
        }
        public void ExtractSynonyms()
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
                wordLine = words[0];
                synonyms.AddRange(words.Skip(1));
            }
            language.Value = wordLine;
            language.Synonysms = synonyms;
        }
        public void ProceedGetType()
        {
            language.Type= language.GetType().ToString();
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
    }
}

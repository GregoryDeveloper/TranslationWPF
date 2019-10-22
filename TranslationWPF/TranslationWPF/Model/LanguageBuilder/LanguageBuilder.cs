using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Helper;

namespace TranslationWPF.Model.LanguageBuilder
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
            modifiedLine = lines[0].Remove(lines[0].Length - NumberOfCharToRemove(lines[0], '('));
            language.Example = StringHelper.CleanWhiteSpaces(lines[1].Replace(")", ""));

        }
        public void SynonymsUnformattedExtraction()
        {
            string wordLine = "";
            string sSynonyms = "";
            List<string> synonyms = new List<string>();
            if (modifiedLine.Contains("(") && modifiedLine.Contains(")") && modifiedLine.Contains('='))
            {
                wordLine = ExtractUntilCaractere(modifiedLine, 0, '(');
                sSynonyms = ExtractUntilCaractere(modifiedLine, modifiedLine.IndexOf('=') + 1, ')');
            }
            else if (modifiedLine.Contains(','))
            {
                wordLine = ExtractUntilCaractere(modifiedLine, 0, ',');
                sSynonyms = ExtractUntilCaractere(modifiedLine, modifiedLine.IndexOf(',') + 1);
            }
            else
                wordLine = modifiedLine;

            language.Value = StringHelper.CleanWhiteSpaces(wordLine);
            language.Synonysms = GetSynonysms(sSynonyms);
        }
        public void ProceedGetType()
        {
            language.Type = language.GetType();
        }
        public void FormattedExtraction()
        {
            language = FormattedStringHelper.FillFromStringRepresention(modifiedLine, language);
        }

        public abstract Language GetResult();

        /// <summary>
        /// Extract a part of the string starting at a given index and ending at a given character if given
        /// </summary>
        /// <param name="line">The string containing the substring to be extracted</param>
        /// <param name="index">The starting index </param>
        /// <param name="caractere">Optionnal: the character the extraction stops when it is met</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a list of synonyms
        /// </summary>
        /// <param name="sSynonysms">The string representation of the synonyms</param>
        /// <returns></returns>
        private static List<string> GetSynonysms(string sSynonysms)
        {
            if (String.IsNullOrEmpty(sSynonysms))
                return new List<string>();

            string[] words = sSynonysms.Split(',');
            words = StringHelper.ExtractFirstCharIfWhiteSpace(words);
            return words.ToList();
        }

        /// <summary>
        /// Get the number of character between the last char of the string and the char specified
        /// return 2 if the specified char is not met
        /// </summary>
        /// <param name="s">The string getting processed</param>
        /// <param name="c">The character to be met</param>
        /// <returns></returns>
        private static int NumberOfCharToRemove(string s, char c)
        {
            int i = s.Length - 1;

            while (i > -1 && s[i] != c)
                i--;

            return i != -1 && (s.Length) - i > 2 ? (s.Length) - i : 2;
        }

        #region formatted import

       

        #endregion  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslationWPF.Model;

namespace TranslationWPF.Helper
{
    public static class FormattedStringHelper
    {
        #region constances
        private const string valuePrefix = "V";
        private const string commentPrefix = "C";
        private const string examplePrefix = "E";
        private const string synonymPrefix = "S";
        private const string typePrefix = "T";

        private const string commentRepresentation = "{" + commentPrefix + "=";
        private const string exampleRepresentation = "{" + examplePrefix + "=";
        private const string synonymRepresentation = "{" + synonymPrefix + "= ";
        private const string typeRepresentation = "{" + typePrefix + "= ";
        #endregion

        public static Language FillFromStringRepresention(string sRepresentation, Language language)
        {
            string[] elements = SplitLine(sRepresentation, '{', '}');

            foreach (string item in elements)
            {
                string[] arr = item.Split('=');
                switch (arr[0])
                {
                    case valuePrefix:
                        language.Value = StringHelper.CleanWhiteSpaces(arr[1]);
                        break;
                    case commentPrefix:
                        language.Comment = StringHelper.CleanWhiteSpaces(arr[1]);
                        break;
                    case examplePrefix:
                        language.Example = StringHelper.CleanWhiteSpaces(arr[1]);
                        break;
                    case synonymPrefix:
                        List<string> synonysms = String.IsNullOrEmpty(arr[1]) 
                            ? new List<string>()
                            : new List<string>(arr[1].Split(','));

                        language.Synonysms = StringHelper.ExtractFirstCharIfWhiteSpace(synonysms.ToArray<string>()).ToList();
                        break;
                    case typePrefix:
                        language.Type = ConvertToTypes(arr[1]);
                        break;
                    default:
                        break;
                }
            }
            return language;
        }

        private static string[] SplitLine(string line, char start, char end)
        {
            int i = 0;
            string value = "";
            List<string> lines = new List<string>();

            (value, i) = GetValue(line, start);
            lines.Add(value);
            lines.AddRange(GetInformations(line, i, start, end));

            return lines.ToArray();
        }

        private static (string, int) GetValue(string line, char start)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(valuePrefix).Append('=');
            while (line[i] != start)
            {
                sb.Append(line[i]);
                i++;
            }
            return (sb.ToString(), i);
        }
        private static List<string> GetInformations(string line, int index, char start, char end)
        {
            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();

            while (index < line.Length - 1)
            {
                if (line[index] == start)
                {
                    sb.Clear();
                    while (index + 1 < line.Length - 1 && line[index + 1] != end)
                    {
                        sb.Append(line[index + 1]);
                        index++;
                    }
                    lines.Add(sb.ToString());

                }
                else
                    index++;
            }
            return lines;
        }

        public static string GetStringRepresentation(Language language)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(language.Value).Append(" ");
            sb.Append(commentRepresentation).Append(language.Comment).Append("}");
            sb.Append(exampleRepresentation).Append(language.Example).Append("}");
            sb.Append(typeRepresentation).Append(language.Type).Append("}");
            sb.Append(synonymRepresentation).Append(GetSynonymsString(language.Synonysms)).Append("}");

            return sb.ToString();

        }

        /// <summary>
        /// Add some extra whiite spaces to a word. eg: parameter = "language", 20 => result language + 12 white spaces
        /// </summary>
        /// <param name="word">the word on which the extra spaces are added</param>
        /// <param name="boundary"> the boundary set the length of the word + extra spaces</param>
        /// <returns>the word with the extra spaces specified</returns>
        public static string GetWordWithWhiteSpaces(string word,int boundary)
        {
            StringBuilder sb = new StringBuilder();

            int extraWhiteSpaces = boundary - word.Length;
            sb.Append(' ', extraWhiteSpaces);
            return sb.ToString();
        }

        private static string GetSynonymsString(List<string> synonysms)
        {
            string s = "";

            if (synonysms.Count == 0)
                return "";

            synonysms.ForEach(sy => s += sy + ",");
            s = s.Remove(s.Length - 1);
            return s;
        }

        private static Language.Types ConvertToTypes(string type)
        {
            switch (type)
            {
                case "noun":
                    return Language.Types.noun;
                case "verb":
                    return Language.Types.verb;
                case "phrasalVerb":
                    return Language.Types.phrasalVerb;
                case "adjective":
                    return Language.Types.adjective;
                case "adverb":
                    return Language.Types.adverb;
                case "undefined":
                    return Language.Types.undefined;
                default:
                    return Language.Types.undefined;
            }
        }

    }
}

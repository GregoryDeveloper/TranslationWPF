using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public abstract class Language
    {
        #region constances
        protected const string valuePrefix = "V";
        protected const string commentPrefix = "C";
        protected const string examplePrefix = "E";
        protected const string synonymPrefix = "S";
        protected const string typePrefix = "T";

        protected const string commentRepresentation = "{" + commentPrefix + "=";
        protected const string exampleRepresentation = "{" + examplePrefix + "=";
        protected const string synonymRepresentation = "{" + synonymPrefix + "= ";
        protected const string typeRepresentation = "{" + typePrefix + "= ";
        #endregion

        #region Properties
        public enum Types
        {
            noun,
            verb,
            phrasalVerb,
            adjective,
            adverb,
            undefined
        }

        public string Line { get; set; } = "";
        public string Value { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Example { get; set; } = "";
        public List<string> Synonysms { get; set; } = new List<string>();
        public string Type { get; set; } = "";
        #endregion

       

        public abstract Types[] GetTypesAvailables();
        // Guess the type depending on the word value, only available for English
        public abstract new Types GetType();
        public abstract Language GetNewInstance();
        public string GetStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Value).Append(" ");
            sb.Append(commentRepresentation).Append(Comment).Append("}");
            sb.Append(exampleRepresentation).Append(Example).Append("}");
            sb.Append(typeRepresentation).Append(Type).Append("}");
            sb.Append(synonymRepresentation).Append(GetSyonymsString()).Append("}");

            return sb.ToString();

        }
        public void FillFromStringRepresenttion(string sRepresentation)
        {
            Line = sRepresentation;
            string[] elements = SplitLine(sRepresentation, '{', '}');

            foreach (string item in elements)
            {
                string[] arr = item.Split('=');
                switch (arr[0])
                {
                    case valuePrefix:
                        Value = arr[1];
                        break;
                    case commentPrefix:
                        Comment = arr[1];
                        break;
                    case examplePrefix:
                        Example = arr[1];
                        break;
                    case synonymPrefix:
                        Synonysms = new List<string>(arr[1].Split(','));
                        break;
                    case typePrefix:
                        Type = arr[1];
                        break;
                    default:
                        break;
                }
            }
        }

        private string[] SplitLine(string line, char start, char end)
        {
            int i = 0;
            string value = "";
            List<string> lines = new List<string>();

            (value,i) = GetValue(line, start);
            lines.Add(value);
            lines.AddRange(GetInformations(line, i, start, end));
           
            return lines.ToArray();
        }

        private (string,int) GetValue(string line,char start)
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(valuePrefix).Append('=');
            while (line[i] != start)
            {
                sb.Append(line[i]);
                i++;
            }
            return (sb.ToString(),i);
        }
        private List<string> GetInformations(string line, int index,char start, char end)
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

        private string GetSyonymsString()
        {
            string s = "";

            if (Synonysms.Count == 0)
                return "";

            Synonysms.ForEach(sy => s += sy + ",");
            s = s.Remove(s.Length - 1);
            return s;
        }

    }
}

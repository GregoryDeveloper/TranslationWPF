using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public abstract class Language
    {
        protected const string commentPrefix = "{C=";
        protected const string examplePrefix = "{E=";
        protected const string synonymPrefix = "{S=";
        protected const string typePrefix = "{T=";

        public enum Types
        {
            noun,
            verb,
            phrasalVerb,
            adjective,
            adverb,
            undefined
        }

        public string Line { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public string Example { get; set; }
        public List<string> Synonysms { get; set; } = new List<string>();
        public string Type { get; set; }
        public abstract Types[] GetTypesAvailables();
        public abstract new Types GetType();
        public string GetStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Value).Append(" ");
            sb.Append(commentPrefix).Append(Comment).Append("}");
            sb.Append(examplePrefix).Append(Example).Append("}");
            sb.Append(typePrefix).Append(Type).Append("}");
            sb.Append(synonymPrefix).Append(GetSyonymsString()).Append("}");

            return sb.ToString();

        }

        private string GetSyonymsString()
        {
            string s="";

            if (Synonysms.Count == 0)
                return "";

            Synonysms.ForEach(sy => s += sy + ",");
            s=s.Remove(s.Length - 1);
            return s;
        }

    }
}

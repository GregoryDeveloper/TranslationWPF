using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.ViewModel
{
    public class TranslationVM
    {
        public string Language1 { get; set; }
        public string Language2 { get; set; }

        public string Language1Comment { get; set; }
        public string Language2Comment { get; set; }

        public List<string> Language1Synonyms { get; set; } = new List<string>();
        public List<string> Language2Synonyms { get; set; } = new List<string>();

        private string _synonyms1String;
        public string Synonyms1String
        {
            get
            {
                if ((_synonyms1String == null || _synonyms1String == "")&& Language1Synonyms.Count>0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language1Synonyms)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }
                    sb.Remove(sb.Length - 1, 1);
                    _synonyms1String = sb.ToString();
                }
                return _synonyms1String;
            }
        }

        private string _synonyms2String;
        public string Synonyms2String
        {
            get
            {
                if ((_synonyms2String == null || _synonyms2String == "") && Language2Synonyms.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language2Synonyms)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }
                    sb.Remove(sb.Length - 1, 1);
                    _synonyms2String = sb.ToString();
                }
                return _synonyms2String;
            }
        }


        public string Language1Example { get; set; }
        public string Language2Example { get; set; }

        public string Language1Type { get; set; }
        public string Language2Type { get; set; }



        public string Line { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;

namespace TranslationWPF.ViewModel
{
    public class TranslationVM
    {

        public Translation Translation { get; set; }

        public Language Language1 { get; set; }
        public Language Language2 { get; set; }


        private string _synonyms1String;
        public string Synonyms1String
        {
            get
            {
                if ((_synonyms1String == null || _synonyms1String == "") && Language1.Synonysms.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language1.Synonysms)
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
                if ((_synonyms2String == null || _synonyms2String == "") && Language2.Synonysms.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string item in Language2.Synonysms)
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


    }
}

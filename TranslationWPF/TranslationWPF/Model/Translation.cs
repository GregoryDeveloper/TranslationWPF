using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class Translation
    {
        public string Line { get; set; }
        public List<Language> Languages { get; set; } = new List<Language>();


        public string GetTranslationStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Languages[0].GetStringRepresentation()).
               Append("=").
               Append(Languages[1].GetStringRepresentation());

            return sb.ToString();
        }



    }
}

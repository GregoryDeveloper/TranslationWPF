using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Helper;

namespace TranslationWPF.Model
{
    public class Translation
    {
        public string Line { get; set; }
        public List<Language> Languages { get; set; } = new List<Language>();

        public Translation(params Language[] languages)
        {
            Array.ForEach(languages, item => Languages.Add(item));
        }

        public string GetTranslationStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FormattedStringHelper.GetStringRepresentation(Languages[0])).
               Append("=").
               Append(FormattedStringHelper.GetStringRepresentation(Languages[1]));

            return sb.ToString();
        }



    }
}

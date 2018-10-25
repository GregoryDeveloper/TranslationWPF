using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class Translation
    {
        public string Line { get; set; }
        public List<Language> Translations { get; set; } = new List<Language>();
    }
}

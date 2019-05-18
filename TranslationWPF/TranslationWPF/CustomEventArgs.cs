using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Model;

namespace TranslationWPF
{
    public class CustomEventArgs
    {
            private List<Translation> translations;
            public List<Translation> Translations
            {
                get { return translations; }
            }
       
            public CustomEventArgs(List<Translation> translations)
            {
                this.translations = translations;
            }
        
    }
}

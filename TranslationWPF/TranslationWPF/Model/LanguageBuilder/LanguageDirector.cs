using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class LanguageDirector
    {
        public void Construct(LanguageBuilder languageBuilder)
        {
            languageBuilder.ExtractComment();
            languageBuilder.ExtractExample();
            languageBuilder.ExtractSynonyms();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model.LanguageBuilder
{
    public class LanguageDirector
    {
        public void ConstructUnformattedImport(LanguageBuilder languageBuilder)
        {
            languageBuilder.ExampleUnformattedExtraction();
            languageBuilder.CommenUnformattedExtraction();
            languageBuilder.SynonymsUnformattedExtraction();
            languageBuilder.ProceedGetType();
        }

        public void ConstructformattedImport(LanguageBuilder languageBuilder)
        {
            languageBuilder.FormattedExtraction();
        }
    }
}

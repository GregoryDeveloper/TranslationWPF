
namespace TranslationWPF.Model
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

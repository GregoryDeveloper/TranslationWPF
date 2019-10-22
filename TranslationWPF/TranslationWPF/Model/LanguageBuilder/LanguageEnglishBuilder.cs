namespace TranslationWPF.Model.LanguageBuilder
{
    public class LanguageEnglishBuilder : LanguageBuilder
    {
        private LanguageEnglishBuilder()
        {

        }

        public LanguageEnglishBuilder(string line)
        {
            modifiedLine = line;
            language = new English();
        }

        public override Language GetResult()
        {
            return language;
        }
    }
}

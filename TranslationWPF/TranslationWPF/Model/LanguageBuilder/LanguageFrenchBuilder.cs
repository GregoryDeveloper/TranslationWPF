namespace TranslationWPF.Model
{
    public class LanguageFrenchBuilder : LanguageBuilder
    {
        private LanguageFrenchBuilder()
        {

        }
        public LanguageFrenchBuilder(string line)
        {
            modifiedLine = line;
            language = new French();

        }

        public override Language GetResult()
        {
            return language;
        }
    }
}

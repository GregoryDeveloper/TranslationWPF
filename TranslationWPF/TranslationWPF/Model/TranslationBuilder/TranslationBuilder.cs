namespace TranslationWPF.Model
{
    public abstract class TranslationBuilder
    {
        protected string line;
        public abstract void EFTranslation();
        public abstract Translation GetResult();
    }
}

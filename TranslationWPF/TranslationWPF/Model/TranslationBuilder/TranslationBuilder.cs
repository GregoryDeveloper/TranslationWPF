﻿namespace TranslationWPF.Model
{
    public abstract class TranslationBuilder
    {
        protected string line;
        protected Translation translation = new Translation();
        public abstract void EFTranslation();
        public abstract Translation GetResult();
    }
}

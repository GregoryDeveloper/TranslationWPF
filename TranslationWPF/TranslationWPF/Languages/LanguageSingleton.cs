using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace TranslationWPF.Languages
{
    public sealed class LanguageSingleton
    {

        private static LanguageSingleton instance = null;
        public static LanguageSingleton Instance
        {
            get { return instance == null ? new LanguageSingleton() : instance; }
        }

        public ResourceManager ResourceManager{ get; private set; }
        public CultureInfo CultureInfo{ get; private set; }

        private LanguageSingleton()
        {
            ResourceManager = new ResourceManager("TranslationWPF.Languages.langres", Assembly.GetExecutingAssembly());
            CultureInfo = Thread.CurrentThread.CurrentCulture;
        }
    }
}

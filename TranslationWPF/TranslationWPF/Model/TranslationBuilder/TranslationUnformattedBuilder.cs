using System.Linq;
using TranslationWPF.Model.LanguageBuilder;
using TranslationWPF.Model.TranslationBuilder;

namespace TranslationWPF.Model.TranslationBuilder
{
    public class TranslationUnformattedBuilder : TranslationBuilder
    {

        private TranslationUnformattedBuilder()
        {

        }

        public TranslationUnformattedBuilder(string l)
        {
            line = l;
        }

        public override void EFTranslation()
        {
            string english;
            string french;
            line = ExtractionHelper.RemoveTabs(line);
            translation.Line = line;

            (english, french) = Splitline(line);

            LanguageDirector director = new LanguageDirector();
            LanguageBuilder.LanguageBuilder englishBuilder = new LanguageEnglishBuilder(english);
            LanguageBuilder.LanguageBuilder frenchBuilder = new LanguageFrenchBuilder(french);

            director.ConstructUnformattedImport(englishBuilder);
            director.ConstructUnformattedImport(frenchBuilder);

            translation.Languages.Add(englishBuilder.GetResult());
            translation.Languages.Add(frenchBuilder.GetResult());

        }
        public override Translation GetResult()
        {
            return translation;
        }

        public (string, string) Splitline(string line)
        {
            string[] splitLine = line.Split('=');

            switch (splitLine.Length)
            {
                case 2:
                    return (splitLine[0], splitLine[1]);
                case 3:
                    if (splitLine[0].Last() == '(')
                        return (splitLine[0] + "=" + splitLine[1], splitLine[2]);
                    else
                        return (splitLine[0], splitLine[1] + "=" + splitLine[2]);
                case 4:
                    return (splitLine[0] + "=" + splitLine[1], splitLine[2] + "=" + splitLine[3]);

                default:
                    break;

            }
            return ("", "");
        }

    }
}

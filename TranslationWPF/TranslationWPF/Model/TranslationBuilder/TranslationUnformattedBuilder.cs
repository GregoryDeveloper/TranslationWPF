using System.Linq;
using System.Text;

namespace TranslationWPF.Model
{
    public class TranslationUnformattedBuilder : TranslationBuilder
    {
        Translation translation = new Translation();

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
            line = RemoveTabs(line);
            translation.Line = line;

            (english, french) = Splitline(line);

            LanguageDirector director = new LanguageDirector();
            LanguageBuilder englishBuilder = new LanguageEnglishBuilder(english);
            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder(french);

            director.Construct(englishBuilder);
            director.Construct(frenchBuilder);

            translation.Translations.Add(englishBuilder.GetResult());
            translation.Translations.Add(frenchBuilder.GetResult());

        }
        public override Translation GetResult()
        {
            return translation;
        }

        private string RemoveTabs(string line)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {

                if (line[i] != '\t')
                    sb.Append(line[i]);

            }
            return sb.ToString();
        }
        private (string,string) Splitline(string line)
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

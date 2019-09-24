using System.Text;

namespace TranslationWPF.Model
{
    public class TranslationFormattedBuilder : TranslationBuilder
    {
        const int equalSymbolCount = 5;

        private TranslationFormattedBuilder()
        {

        }
        public TranslationFormattedBuilder(string l)
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
            LanguageBuilder englishBuilder = new LanguageEnglishBuilder(english);
            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder(french);

            director.ConstructformattedImport(englishBuilder);
            director.ConstructformattedImport(frenchBuilder);

            translation.Languages.Add(englishBuilder.GetResult());
            translation.Languages.Add(frenchBuilder.GetResult());
        }

        public override Translation GetResult()
        {
            return translation;
        }



        private (string, string) Splitline(string line)
        {
            int splitIndex = GetSplitIndex(line);

            return (ExtractString(0, splitIndex-1),ExtractString(splitIndex+1,line.Length-1));

        }
        private string ExtractString(int startIndex, int endIndex)
        {
            StringBuilder sb = new StringBuilder();

            while (startIndex < endIndex+1)
            {
                sb.Append(line[startIndex]);
                startIndex++;
            }

            return sb.ToString();
        }
        private int GetSplitIndex(string line)
        {
            int i = 0;
            int index = 0;

            while (index < line.Length-1)
            {
                if (line[index].CompareTo('=') == 0)
                {
                    i++;
                    if (i == equalSymbolCount)
                        return index;
                }
                index++;
            }

            throw new System.InvalidOperationException();
        }

    }
}

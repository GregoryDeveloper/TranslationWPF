using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslationWPF.Helper;

namespace TranslationWPF.Model
{
    public class Translation
    {
        // TODO: refacto useless now check
        private static int count { get; set; } = 0;
        // TODO: refacto useless now check
        public int Id { get; }
        public string Line { get; set; }
        public List<Language> Languages { get; set; } = new List<Language>();
        public Translation(params Language[] languages)
        {
            Array.ForEach(languages, item => Languages.Add(item));
            Id = count;
            count++;
        }

        public string GetTranslationStringRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FormattedStringHelper.GetStringRepresentation(Languages[0])).
               Append("=").
               Append(FormattedStringHelper.GetStringRepresentation(Languages[1]));

            return sb.ToString();
        }

        public bool HasLanguage(Language.Languages language)
        {
            return Languages.Any(l => l.GetLanguage() == language);
        }

        public bool HasLanguages(params Language.Languages[] languages)
        {
            foreach (Language.Languages language in languages)
            {
               if(!Languages.Any(l => l.GetLanguage() == language))
                    return false;
            }

            return true;
        }

        public Language GetLanguage(Language.Languages language)
        {
            return Languages.Where(l => l.GetLanguage() == language)
                     .FirstOrDefault();
        }

        public List<Language.Languages> GetCurrentLanguages()
        {
            List<Language.Languages> languages = new List<Language.Languages>();
            Languages.ForEach(l => languages.Add(l.GetLanguage()));

            return languages;
        }

        public List<Language.Languages> GetMissingLanguages()
        {
            List<Language.Languages> currentlanguages = GetCurrentLanguages();
            List<Language.Languages> languages = Language.GetLanguages();

            return languages.Where(l => !currentlanguages.Contains(l)).ToList();
        }

        public string GetLanguageValue(Language.Languages language)
        {
            return Languages.Where(l => l.GetLanguage() == language)
                     .Select(l => l.Value)
                     .First();
        }

        public void SetLanguageType(Language.Languages language, Language.Types type)
        {
            foreach (Language l in Languages)
            {
                if (l.Is(language))
                    l.Type = type;            
            }
        }

        public void SetLanguageSynonysms(Language.Languages language, List<String> synonysms)
        {
            foreach (Language l in Languages)
            {
                if (l.Is(language))
                    l.Synonysms = synonysms;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Languages[0].Value)
              .Append(FormattedStringHelper.GetWordWithWhiteSpaces(sb.ToString(), Constants.SPACE_NUMBER_BOUNDARY))
              .Append(Constants.SEPARATOR)
              .Append(' ', Constants.SPACE_NUMBER)
              .Append(Languages[1].Value);

            foreach (string item in Languages[1].Synonysms)
            {
                sb.Append(Constants.SYNONYM_SEPARATOR)
                  .Append(item);
            }


            return sb.ToString();
        }
        
    }
}

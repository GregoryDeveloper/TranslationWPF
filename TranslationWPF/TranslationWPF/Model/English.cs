using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class English: Language
    {

        private string[] adjectivesEnding = { "able","ible","al","an","ar","ent","ful","ic","ical","ine","ile","ive","less","ous","some" };

        public English() : base() { ObjType = LanguageConstant.english; }
        public English(string value, string comment, string example, Types type, List<string> synonysms)
            :base( value,  comment,  example, type, synonysms) { ObjType = LanguageConstant.english; }

        public override Types GetType()
        {
            if (IsVerb(Value))
            {
                if (IsPhrasalVerb(Value))
                    return Types.phrasalVerb;

                return Types.verb;
            }

            if (IsNoun(Value))           
                return Types.noun;           

            if (IsAdverb(Value))
                return Types.adverb;

            if (IsAdjective(Value))
                return Types.adjective;

            return Types.undefined;
        }

        public override Types[] GetTypesAvailables()
        {
            return new Types[] { Types.noun, Types.verb,Types.phrasalVerb, Types.adjective, Types.adverb };
        }

        private bool IsVerb(string s)
        {
            if (s.Length < 2)
                return false;

            if (s.Substring(0, 2) == "to" || s.Substring(0, 2) == "To")
                return true;

            return false;
        }
        // Assumes it is already a verb
        private bool IsPhrasalVerb(string s)
        {
            string[] splitVerb = s.Split(' ');

            if (splitVerb.Length > 2 || s.Contains('['))
                return true;

            return false;
        }
        private bool IsNoun(string s)
        {
            if (s.Length < 2)
                return false;

            if ( s.Substring(0, 2).ToLower() == "an" || s.Substring(0, 1).ToLower() == "a"|| s.Substring(0, 3).ToLower() == "the")
                return true;
            return false;
        }
        private bool IsAdverb(string s)
        {
            if (s.Length < 2)
                return false;

            if (s.Substring(s.Length-3, 2) == "ly")
                return true;
            return false;
        }
        private bool IsAdjective(string s)
        {
            foreach (string ending in adjectivesEnding)
            {
                if (s.Length < ending.Length)
                    return false;
                string temp = s.Substring(s.Length - (ending.Length - 1));
                if (temp == ending)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///  This function has for intend to create the same language as the one you are manipulating
        /// </summary>
        /// <returns></returns>
        public override Language GetNewInstance()
        {
            return new English();
        }

        public override Languages GetLanguage()
        {
            return Languages.English;
        }

        public override bool Is(Languages language)
        {
           return language == Languages.English? true : false ;
        }
    }
}

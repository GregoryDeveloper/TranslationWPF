using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TranslationWPF.Model.Language;

namespace TranslationWPF.Model
{
    public class Spanish: Language
    {
        public Spanish() : base() { ObjType = LanguageConstant.spanish; }
        public Spanish(string value, string comment, string example, Types type, List<string> synonysms)
            : base(value, comment, example, type, synonysms) { ObjType = LanguageConstant.spanish; }

        public override Types GetType()
        {
            return Types.undefined;
        }

        public override Types[] GetTypesAvailables()
        {
            return new Types[] { Types.noun, Types.verb, Types.adjective, Types.adverb };
        }

        public override Language GetNewInstance()
        {
            return new Spanish();
        }

        public override Languages GetLanguage()
        {
            return Languages.Spanish;
        }

        public override bool Is(Languages language)
        {
            return language == Languages.Spanish ? true : false;
        }
    }
}

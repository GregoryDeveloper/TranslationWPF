using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class French : Language
    {

        public French() : base() { ObjType = LanguageConstant.french; }
        public French(string value, string comment, string example, Types type, List<string> synonysms)
            : base(value, comment, example, type, synonysms) { ObjType = LanguageConstant.french; }

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
            return new French();
        }

        public override Languages GetLanguage()
        {
            return Languages.French;
        }

        public override bool Is(Languages language)
        {
            return language == Languages.French;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class French : Language
    {

        public French() : base() { ObjType = Constant.french; }
        public French(string value, string comment, string example, Types type, List<string> synonysms)
            : base(value, comment, example, type, synonysms) { ObjType = Constant.french; }

        public override Types GetType()
        {
            return Types.undefined;
            //throw new NotImplementedException();
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
    }
}

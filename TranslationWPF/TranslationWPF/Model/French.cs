using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public class French : Language
    {


        public override Types GetType()
        {
            return Types.undefined;
            //throw new NotImplementedException();
        }

        public override Types[] GetTypesAvailables()
        {
            return new Types[] { Types.noun, Types.verb, Types.adjective, Types.adverb };
        }
    }
}

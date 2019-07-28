using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Exceptions
{
    public class NoItemException: Exception
    {
        public NoItemException()
        {
        }

        public NoItemException(string message)
            : base(message)
        {
        }

        public NoItemException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}

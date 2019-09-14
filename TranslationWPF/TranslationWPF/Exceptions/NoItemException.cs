using System;

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

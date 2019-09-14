using System;

namespace TranslationWPF.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// Clean the white spaces at the beginning and at the end of the string
        /// </summary>
        /// <param name="array">The array containing the strings</param>
        /// <returns></returns>
        public static string[] ExtractFirstCharIfWhiteSpace(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = CleanWhiteSpaces(array[i]);
            }
            return array;
        }
        public static string CleanWhiteSpaces(string s)
        {
            if (String.IsNullOrEmpty(s))
                return s;

            if (Char.IsWhiteSpace(s[0]))
                s = s.Remove(0, 1);
            if (Char.IsWhiteSpace(s[s.Length - 1]))
                s = s.Remove(s.Length - 1, 1);

            return s;
        }
    }
}

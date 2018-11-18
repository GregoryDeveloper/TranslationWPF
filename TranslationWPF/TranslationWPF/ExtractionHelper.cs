using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF
{
    public static class ExtractionHelper
    {
        public static string RemoveTabs(string line)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {

                if (line[i] != '\t')
                    sb.Append(line[i]);

            }
            return sb.ToString();
        }

    }
}

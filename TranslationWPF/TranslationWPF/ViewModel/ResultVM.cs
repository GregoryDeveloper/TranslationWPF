using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.ViewModel
{
    public class ResultVM
    {
        public TranslationVM Translation { get; set; }
        public string CorrectValues { get; set; } = "";

        public ResultVM(TranslationVM translation)
        {
            Translation = translation;
            CorrectValues = GetRightValues(Translation);
        }

        private string GetRightValues(TranslationVM translation)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(translation.Language1.Value);
            translation.Language1.Synonysms.ForEach(item =>
            {
                sb.Append("/")
                    .Append(item);
            });
           return sb.ToString();
        }
    }
}

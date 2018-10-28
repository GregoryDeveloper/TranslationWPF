using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public abstract class Language
    {
        public string Line { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public string Example { get; set; }
        public List<string> Synonysms { get; set; } = new List<string>();
        public string Type { get; set; }

    }
}

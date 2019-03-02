﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationWPF.Model
{
    public abstract class Language
    {
       

        #region Properties
        public enum Types
        {
            noun,
            verb,
            phrasalVerb,
            adjective,
            adverb,
            undefined
        }

        //public string Line { get; set; } = "";
        public string Value { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Example { get; set; } = "";
        public Types Type { get; set; } = Types.undefined;
        public List<string> Synonysms { get; set; } = new List<string>();
        #endregion

        #region Constructors
        protected Language()
        {

        }
        protected Language(string value, string comment, string example, Types type ,List<string> synonysms)
        {
            this.Value = value;
            this.Comment = comment;
            this.Example = example;
            this.Type = type;
            this.Synonysms = synonysms;
        }
        #endregion

        public abstract Types[] GetTypesAvailables();
        // Guess the type depending on the word value, only available for English
        public abstract new Types GetType();
        public abstract Language GetNewInstance();

       

    }
}

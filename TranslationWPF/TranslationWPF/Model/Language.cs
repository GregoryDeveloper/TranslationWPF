﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationWPF.Converter;
using System.ComponentModel;

namespace TranslationWPF.Model
{
    [JsonConverter(typeof(BaseConverter))]
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
        public enum Languages
        {
            [Description("English")]
            English =1,
            [Description("French")]
            French = 2,
            [Description("Spanish")]
            Spanish = 3
        }

        // used for serialization
        public int ObjType { get; set; }
        public int Id { get; set; }

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
        public abstract Languages GetLanguage();
        public abstract bool Is(Language.Languages language);
        public static List<Languages> GetLanguages()
        {
            return Enum.GetValues(typeof(Language.Languages)).Cast<Language.Languages>().ToList();
        }

        public static Language CreateLanguage(Languages language)
        {
            switch (language)
            {
                case Languages.English:
                    return new English();
                case Languages.French:
                    return new French();
                case Languages.Spanish:
                    return new Spanish();
                default:
                    return new English();
            }
        }

        // check if the value in parameter corresponds to the value or to a synonym.
        public bool Validate(string value)
        {
            if (value == this.Value)
                return true;

            return Synonysms.Any(v => v == value);
        }
       

    }
}

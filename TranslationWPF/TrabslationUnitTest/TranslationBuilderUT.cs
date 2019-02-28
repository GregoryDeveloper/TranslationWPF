using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslationWPF.Model;
using System.Linq;
using System.Collections.Generic;

namespace TrabslationUnitTest
{
    [TestClass]
    public class TranslationBuilderUT
    {
        //to run into(= come across)                        =		rencontrer par hasard,croiser, tomber sur/
          [TestMethod]
        public void ModidifiedLineUnformattedExtractionSuccessCaseTest1()
        {
            string expected = "to run into (= come across)=rencontrer par hasard,croiser, tomber sur";
            string line = "to run into (= come across)						=		rencontrer par hasard,croiser, tomber sur";

            TranslationDirector director = new TranslationDirector();
            TranslationBuilder translationBuilder;
            translationBuilder = new TranslationUnformattedBuilder(line);
            director.Construct(translationBuilder);
            string actual = translationBuilder.GetResult().Line;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnformattedExtractionSuccessCaseTest1()
        {
            Translation expected = new Translation(
                new English("Mere",
                    "",
                    "",
                    Language.Types.undefined,
                    new List<string>() { "just","no more than" }),

                new French("simple",
                "", 
                "l’ avion s est écrasé seulement 10 minutes après le décollage",
                Language.Types.undefined, new List<string>() { "ne … que …", "seulement" })
                );
            string line = "Mere (= just, no more than)= simple, ne … que …, seulement ( ex: l’ avion s est écrasé seulement 10 minutes après le décollage)";

            TranslationDirector director = new TranslationDirector();
            TranslationBuilder translationBuilder;
            translationBuilder = new TranslationUnformattedBuilder(line);
            director.Construct(translationBuilder);
            Translation actual = translationBuilder.GetResult();

            Assert.IsTrue(AreSameLanguages(expected.Languages[0], actual.Languages[0]) && 
                AreSameLanguages(expected.Languages[1], actual.Languages[1]));
        }

        private bool AreSameLanguages(Language expected, Language actual)
        {
            if (expected.Value != actual.Value)
                return false;
            if (expected.Comment != actual.Comment)
                return false;
            if (expected.Example != actual.Example)
                return false;
            if (expected.Type != actual.Type)
                return false;
            if (!expected.Synonysms.SequenceEqual(actual.Synonysms))
                return false;

            return true;
        }
    }
}

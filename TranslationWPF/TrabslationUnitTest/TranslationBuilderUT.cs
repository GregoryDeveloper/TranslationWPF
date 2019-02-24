using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslationWPF.Model;

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
    }
}

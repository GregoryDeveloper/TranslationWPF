using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslationWPF.Model;

namespace TrabslationUnitTest
{
    [TestClass]
    public class LangageBuilderUT
    {
        [TestMethod]
        public void CommentUnformattedExtractionSuccessCaseTest1()
        {
            string expected = "vacances";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("reporter à(vacances)");

            director.ConstructUnformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Comment;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CommentUnformattedExtractionSuccessCaseTest2()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();
            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to carry [sth] over");

            director.ConstructUnformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Comment;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExempleUnformattedExtractionSuccessCaseTest1()
        {
            string expected = "you must carry out the instructions carefully";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("effectuer, réaliser, mener, mener à éxécution, suivre ex: you must carry out the instructions carefully");

            director.ConstructUnformattedImport(frenchBuilder);
            string actual = frenchBuilder.GetResult().Example;
  
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExempleUnformattedExtractionSuccessCaseTest2()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();
            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to carry out");

            director.ConstructUnformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Example;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValueUnformattedExtractionSuccessCaseTest1()
        {
            string expected = "to look [sb] over";
            LanguageDirector director = new LanguageDirector();
            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over");

            director.ConstructUnformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Value;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValueUnformattedExtractionSuccessCaseTest2()
        {
            string expected = "regarder";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder, examiner");

            director.ConstructUnformattedImport(frenchBuilder);
            string actual = frenchBuilder.GetResult().Value;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonymsUnformattedExtractionSuccessCaseTest1()
        {
            List<string> expected = new List<string> { "réaliser", "mener", "mener à éxécution", "suivre" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("effectuer, réaliser, mener, mener à éxécution, suivre ex: you must carry out the instructions carefully");

            director.ConstructUnformattedImport(frenchBuilder);

            List<string> actual = frenchBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonymsUnformattedExtractionSuccessCaseTest2()
        {
            List<string> expected = new List<string> { };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to carry out");
            
            director.ConstructUnformattedImport(englishBuilder);

            List<string> actual = englishBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void PVTypeUnformattedExtractionSuccessCaseTest1()
        {
            string expected = English.Types.phrasalVerb.ToString();
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to call[sb]in");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            string actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PVTypeUnformattedExtractionSuccessCaseTest2()
        {
           string expected = English.Types.phrasalVerb.ToString();
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            string actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void NounTypeUnformattedExtractionSuccessCaseTest1()
        {
            string expected = English.Types.noun.ToString();
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("a briefcase");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            string actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VerbTypeUnformattedExtractionSuccessCaseTest1()
        {
            string expected = English.Types.verb.ToString();
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to faint");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            string actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }


    }
}

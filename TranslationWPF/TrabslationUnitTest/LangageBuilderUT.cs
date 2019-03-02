using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslationWPF.Model;

namespace TrabslationUnitTest
{
    [TestClass]
    public class LangageBuilderUT
    {
        #region unformatted import
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
            Language.Types expected = English.Types.phrasalVerb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to call[sb]in");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PVTypeUnformattedExtractionSuccessCaseTest2()
        {
            Language.Types expected = English.Types.phrasalVerb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void NounTypeUnformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.noun;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("a briefcase");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VerbTypeUnformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.verb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to faint");

            director.ConstructUnformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region formatted import
        #region Comment

        //to look [sb] over {C=}{E=}{T=phrasalVerb}{S=}=regarder {C=}{E=}{T=undefined}{S= examiner}
        [TestMethod]
        public void CommentFormattedImportSuccessCaseTest1()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Comment;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CommentFormattedImportSuccessCaseTest2()
        {
            string expected = "test";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=test}{E=}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Comment;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CommentFormattedImportSuccessCaseTest3()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Comment;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CommentFormattedImportSuccessCaseTest4()
        {
            string expected = "test";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=test}{E=}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Comment;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region example

        [TestMethod]
        public void ExampleFormattedImportSuccessCaseTest1()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Example;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExampleFormattedImportSuccessCaseTest2()
        {
            string expected = "test";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=test}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Example;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExampleFormattedImportSuccessCaseTest3()
        {
            string expected = "";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Example;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ExampleFormattedImportSuccessCaseTest4()
        {
            string expected = "test";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=test}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Example;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Value
       

        [TestMethod]
        public void ValueFormattedImportSuccessCaseTest2()
        {
            string expected = "regarder";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=test}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            string actual = frenchBuilder.GetResult().Value;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValueFormattedImportSuccessCaseTest4()
        {
            string expected = "to look [sb] over";
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=test}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            string actual = englishBuilder.GetResult().Value;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Types

        //to look [sb] over {C=}{E=}{T=phrasalVerb}{S=}=regarder {C=}{E=}{T=undefined}{S= examiner}
        //noun,
        //    verb,
        //    phrasalVerb,
        //    adjective,
        //    adverb,
        //    undefined
        [TestMethod]
        public void PVTypeformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.phrasalVerb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);
            englishBuilder.ProceedGetType();

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void VerbTypeformattedExtractionSuccessCaseTest2()
        {
            Language.Types expected = English.Types.verb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=verb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AdjectiveTypeformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.adjective;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=adjective}{S=}");

            director.ConstructformattedImport(englishBuilder);

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AdverbTypeformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.adverb;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=adverb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UndefinedTypeformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.undefined;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=undefined}{S=}");

            director.ConstructformattedImport(englishBuilder);

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NounTypeformattedExtractionSuccessCaseTest1()
        {
            Language.Types expected = English.Types.noun;
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=}{T=noun}{S=}");

            director.ConstructformattedImport(englishBuilder);

            Language.Types actual = englishBuilder.GetResult().Type;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region Synonysms
        [TestMethod]
        public void SynonysmsFormattedImportSuccessCaseTest1()
        {
            List<string> expected = new List<string> { "examiner" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=}{T=undefined}{S= examiner}");

            director.ConstructformattedImport(frenchBuilder);

            List<string> actual = frenchBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonysmsFormattedImportSuccessCaseTest2()
        {
            List<string> expected = new List<string> { "examiner","test1","test2" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=}{T=undefined}{S= examiner,test1,test2}");

            director.ConstructformattedImport(frenchBuilder);

            List<string> actual = frenchBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        public void SynonysmsFormattedImportSuccessCaseTest3()
        {
            List<string> expected = new List<string> { "" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder frenchBuilder = new LanguageFrenchBuilder("regarder {C=}{E=}{T=undefined}{S=}");

            director.ConstructformattedImport(frenchBuilder);

            List<string> actual = frenchBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonysmsFormattedImportSuccessCaseTest4()
        {
            List<string> expected = new List<string> { };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=test}{T=phrasalVerb}{S=}");

            director.ConstructformattedImport(englishBuilder);

            List<string> actual = englishBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonysmsFormattedImportSuccessCaseTest5()
        {
            List<string> expected = new List<string> { "test1" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=test}{T=phrasalVerb}{S=test1}");

            director.ConstructformattedImport(englishBuilder);

            List<string> actual = englishBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SynonysmsFormattedImportSuccessCaseTest6()
        {
            List<string> expected = new List<string> { "test1", "test2" };
            LanguageDirector director = new LanguageDirector();

            LanguageBuilder englishBuilder = new LanguageEnglishBuilder("to look [sb] over {C=}{E=test}{T=phrasalVerb}{S=test1,test2}");

            director.ConstructformattedImport(englishBuilder);

            List<string> actual = englishBuilder.GetResult().Synonysms;
            CollectionAssert.AreEqual(expected, actual);
        }


        #endregion

        #endregion

    }
}

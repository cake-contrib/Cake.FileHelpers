using NUnit.Framework;
using System;
using Cake.Xamarin.Tests.Fakes;
using Cake.Common.IO;

namespace Cake.FileHelpers.Tests
{
    [TestFixture]
    public class FileHelperTests
    {
        FakeCakeContext context;

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();

            if (context.CakeContext.DirectoryExists ("./testdata"))
                context.CakeContext.CleanDirectory ("./testdata");
            else 
                context.CakeContext.CreateDirectory ("./testdata");
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }

        [Test]
        public void TestWriteAndReadText ()
        {         
            const string file = "./testdata/Text.txt";
            const string contents = "This is a test";

            context.CakeContext.FileWriteText (file, contents);

            var read = context.CakeContext.FileReadText (file);

            Assert.AreEqual (contents, read);
        }

        [Test]
        public void TestWriteAndReadLines ()
        {   
            const string file = "./testdata/Lines.txt";
            var contents = new [] { "This", "is", "a", "test" };

            context.CakeContext.FileWriteLines (file, contents);

            var read = context.CakeContext.FileReadLines (file);

            Assert.IsNotNull (read);
            Assert.AreEqual (contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.AreEqual (contents[i], read[i]);
        }

        [Test]
        public void FindTextInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.FindTextInFiles ("./testdata/*.txt", "Monkey");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);
        }

        [Test]
        public void FindRegexInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.FindRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);
        }


        [Test]
        public void ReplaceTextInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.ReplaceTextInFiles ("./testdata/*.txt", "Monkey", "Tamarin");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.AreEqual (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        [Test]
        public void ReplaceRegexInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.ReplaceRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}", " Tamarin ");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.AreEqual (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }


        public const string PATTERN_FILE_BASE_VALUE = "The {0} makes great software.\nThis is not a surprise.";

        void SetupFiles ()
        {            
            // Setup files
            for (int i = 1; i < 5; i++) {
                context.CakeContext.FileWriteText (
                    string.Format ("./testdata/{0}.txt", i),
                    string.Format (PATTERN_FILE_BASE_VALUE, i == 2 ? "Monkey" : "Ape"));                
            }
        }
    }
}


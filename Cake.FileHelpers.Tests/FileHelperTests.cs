using NUnit.Framework;
using System;
using System.Text;
using Cake.Xamarin.Tests.Fakes;
using Cake.Core.IO;

namespace Cake.FileHelpers.Tests
{
    [TestFixture]
    public class FileHelperTests
    {
        FakeCakeContext context;

		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(FileHelperTests).Assembly.Location);
		}

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();

            var dp = new DirectoryPath ("./testdata");
            var d = context.CakeContext.FileSystem.GetDirectory (dp);

            if (d.Exists)
                d.Delete (true);

            d.Create ();
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
        public void TestWriteAndReadTextWithUTF8Encoding ()
        {
            const string file = "./testdata/Text.txt";
            const string contents = "This is a test. This is a smiley face: ☺";

            context.CakeContext.FileWriteText (file, Encoding.UTF8, contents);

            var read = context.CakeContext.FileReadText (file, Encoding.UTF8);

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
        public void TestWriteAndReadLinesWithUTF8Encoding ()
        {
            const string file = "./testdata/Lines.txt";
            var contents = new[] { "This is a smiley face: ☺", "This is a copyright symbol: ©", "This is a recycle symbol: ♻" };

            context.CakeContext.FileWriteLines (file, Encoding.UTF8, contents);

            var read = context.CakeContext.FileReadLines (file, Encoding.UTF8);

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
        public void FindTextInFilesWithUTF8Encoding ()
        {
            SetupFilesUTF8 ();

            var files = context.CakeContext.FindTextInFiles ("./testdata/*.txt", Encoding.UTF8, "Monkey");

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
        public void FindRegexInFilesWithUTF8Encoding ()
        {
            SetupFilesUTF8 ();

            var files = context.CakeContext.FindRegexInFiles ("./testdata/*.txt", Encoding.UTF8, @"\s{1}Monkey\s{1,}");

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
        public void ReplaceTextInFilesWithUTF8Encoding ()
        {
            SetupFilesUTF8 ();

            var files = context.CakeContext.ReplaceTextInFiles ("./testdata/*.txt", Encoding.UTF8, "Monkey", "Tamarin");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);

            foreach (var f in files)
            {
                var contents = context.CakeContext.FileReadText (f, Encoding.UTF8);
                Assert.AreEqual (string.Format(PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
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

        [Test]
        public void ReplaceRegexInFilesWithUTF8Encoding ()
        {
            SetupFilesUTF8 ();

            var files = context.CakeContext.ReplaceRegexInFiles ("./testdata/*.txt", Encoding.UTF8, @"\s{1}Monkey\s{1,}", " Tamarin ");

            Assert.IsNotNull (files);
            Assert.AreEqual (1, files.Length);

            foreach (var f in files)
            {
                var contents = context.CakeContext.FileReadText (f, Encoding.UTF8);
                Assert.AreEqual (string.Format(PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
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

        void SetupFilesUTF8()
        {
            // Setup files
            for (int i = 1; i < 5; i++)
            {
                context.CakeContext.FileWriteText (
                    string.Format("./testdata/{0}.txt", i),
                    Encoding.UTF8,
                    string.Format(PATTERN_FILE_BASE_VALUE, i == 2 ? "Monkey" : "Ape"));
            }
        }
    }
}


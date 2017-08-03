using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
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

        public const string GROUPS_FILE = "./testdata/Groups.txt";
        public const string GROUPS_FILE_CONTENT = "Hello World! This is A quick Test to Capture multiple Groups.";
        public const string GROUPS_PATTERN = "([A-Z])(\\w+)";

        [Test]
        public void FindRegexMatchesGroupsInFile ()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchesGroups = context.CakeContext.FindRegexMatchesGroupsInFile (GROUPS_FILE, GROUPS_PATTERN, RegexOptions.None);

            Assert.IsNotNull (matchesGroups);
            Assert.AreEqual (matchesGroups.Count, 6);

            foreach (var g in matchesGroups)
                Assert.AreEqual (g.Count, 3);
        }

        [Test]
        public void FindRegexMatchGroupsInFile()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchGroups = context.CakeContext.FindRegexMatchGroupsInFile (GROUPS_FILE, GROUPS_PATTERN, RegexOptions.None);

            Assert.IsNotNull (matchGroups);
            Assert.AreEqual (matchGroups.Count, 3);
        }

        [Test]
        public void FindRegexMatchGroupInFile ()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 2, RegexOptions.None);
            var invalidMatchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 8, RegexOptions.None);

            Assert.IsNotNull (matchGroup);
            Assert.IsNull (invalidMatchGroup);
            Assert.AreEqual (matchGroup.Value, "ello");
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


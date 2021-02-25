using System;
using System.Text.RegularExpressions;
using Cake.Xamarin.Tests.Fakes;
using Cake.Core.IO;
using Xunit;

namespace Cake.FileHelpers.Tests
{
    public class FileHelperTests : IDisposable
    {
        FakeCakeContext context;

        public FileHelperTests()
        {
            context = new FakeCakeContext();

            var dp = new DirectoryPath("./testdata");
            var d = context.CakeContext.FileSystem.GetDirectory(dp);
         
            if (d.Exists)
                d.Delete(true);

            d.Create();
        }
        
        public void Dispose ()
        {
            context.DumpLogs ();
        }

        [Fact]
        public void TestWriteAndReadText ()
        {         
            const string file = "./testdata/Text.txt";
            const string contents = "This is a test";

            context.CakeContext.FileWriteText (file, contents);

            var read = context.CakeContext.FileReadText (file);

            Assert.Equal (contents, read);
        }

        [Fact]
        public void TestWriteAndReadLines ()
        {   
            const string file = "./testdata/Lines.txt";
            var contents = new [] { "This", "is", "a", "test" };

            context.CakeContext.FileWriteLines (file, contents);

            var read = context.CakeContext.FileReadLines (file);

            Assert.NotNull (read);
            Assert.Equal (contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.Equal (contents[i], read[i]);
        }

        [Fact]
        public void FindTextInFilesGlob ()
        {
            SetupFiles ();

            var files = context.CakeContext.FindTextInFiles ("./testdata/*.txt", "Monkey");

            Assert.NotNull (files);
            Assert.Single(files);
        }

        [Fact]
        public void FindTextInFiles ()
        {
            SetupFiles();

            var files = context.CakeContext.Globber.GetFiles ("./testdata/*.txt");

            var monkeyFiles = context.CakeContext.FindTextInFiles (files, "Monkey");

            Assert.NotNull (monkeyFiles);
            Assert.Single (monkeyFiles);
        }

        [Fact]
        public void FindRegexInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.FindRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}");

            Assert.NotNull (files);
            Assert.Single (files);
        }


        [Fact]
        public void ReplaceTextInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.ReplaceTextInFiles ("./testdata/*.txt", "Monkey", "Tamarin");

            Assert.NotNull (files);
            Assert.Single (files);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.Equal (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        [Fact]
        public void ReplaceRegexInFiles ()
        {
            SetupFiles ();

            var files = context.CakeContext.ReplaceRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}", " Tamarin ");

            Assert.NotNull (files);
            Assert.Single(files);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.Equal (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        public const string GROUPS_FILE = "./testdata/Groups.txt";
        public const string GROUPS_FILE_CONTENT = "Hello World! This is A quick Test to Capture multiple Groups.";
        public const string GROUPS_PATTERN = "([A-Z])(\\w+)";

        [Fact]
        public void FindRegexMatchesGroupsInFile ()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchesGroups = context.CakeContext.FindRegexMatchesGroupsInFile (GROUPS_FILE, GROUPS_PATTERN, RegexOptions.None);

            Assert.NotNull (matchesGroups);
            Assert.Equal (6, matchesGroups.Count);

            foreach (var g in matchesGroups)
                Assert.Equal (3, g.Count);
        }

        [Fact]
        public void FindRegexMatchGroupsInFile()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchGroups = context.CakeContext.FindRegexMatchGroupsInFile (GROUPS_FILE, GROUPS_PATTERN, RegexOptions.None);

            Assert.NotNull (matchGroups);
            Assert.Equal (3, matchGroups.Count);
        }

        [Fact]
        public void FindRegexMatchGroupInFile ()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 2, RegexOptions.None);
            var invalidMatchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 8, RegexOptions.None);

            Assert.NotNull (matchGroup);
            Assert.Null (invalidMatchGroup);
            Assert.Equal ("ello", matchGroup.Value);
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


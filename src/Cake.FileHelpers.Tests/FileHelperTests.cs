using System;
using System.Text.RegularExpressions;
using Cake.Xamarin.Tests.Fakes;
using Cake.Core.IO;
using Xunit;
using System.Text;

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

        public void Dispose()
        {
            context.DumpLogs();
        }

        [Fact]
        public void TestWriteAndReadText()
        {
            const string file = "./testdata/Text.txt";
            const string contents = "This is a test";

            context.CakeContext.FileWriteText (file, contents);

            var read = context.CakeContext.FileReadText (file);

            Assert.Equal (contents, read);
        }

        [Fact]
        public void TestWriteAndReadLines()
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
        public void TestAppendToNewFileAndReadText()
        {
            const string file = "./testdata/Text.txt";
            const string contents = "This is a test";

            context.CakeContext.FileAppendText(file, contents);

            var read = context.CakeContext.FileReadText(file);

            Assert.Equal(contents, read);
        }

        [Fact]
        public void TestAppendToExistingFileAndReadText()
        {
            const string file = "./testdata/Text.txt";
            const string contents1 = "This is ";
            const string contents2 = "a test";

            context.CakeContext.FileAppendText(file, contents1);
            context.CakeContext.FileAppendText(file, contents2);

            var read = context.CakeContext.FileReadText(file);

            Assert.Equal(contents1 + contents2, read);
        }

        [Fact]
        public void TestAppendToNewFileAndReadTextWithUTF8Encoding()
        {
            const string file = "./testdata/Text.txt";
            const string contents = "Monkey🐒";

            context.CakeContext.FileAppendText(file, Encoding.UTF8, contents);

            var read = context.CakeContext.FileReadText(file, Encoding.UTF8);

            Assert.Equal(contents, read);
        }

        [Fact]
        public void TestAppendToExistingFileAndReadTextWithUTF8Encoding()
        {
            const string file = "./testdata/Text.txt";
            const string contents1 = "Monkey";
            const string contents2 = "🐒";

            context.CakeContext.FileAppendText(file, Encoding.UTF8, contents1);
            context.CakeContext.FileAppendText(file, Encoding.UTF8, contents2);

            var read = context.CakeContext.FileReadText(file, Encoding.UTF8);

            Assert.Equal(contents1 + contents2, read);
        }

        [Fact]
        public void TestAppendLinesToNewFileAndReadLines()
        {
            const string file = "./testdata/Text.txt";
            var contents = new[] { "This", "is", "a", "test" };

            context.CakeContext.FileAppendLines(file, contents);

            var read = context.CakeContext.FileReadLines(file);

            Assert.Equal(contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.Equal(contents[i], read[i]);
        }

        [Fact]
        public void TestAppendLinesToExistingFileAndReadLines()
        {
            const string file = "./testdata/Text.txt";
            var contents = new[] { "This", "is", "a", "test" };
            var contents1 = new[] { contents[0], contents[1] };
            var contents2 = new[] { contents[2], contents[3] };

            context.CakeContext.FileAppendLines(file, contents1);
            context.CakeContext.FileAppendLines(file, contents2);

            var read = context.CakeContext.FileReadLines(file);

            Assert.Equal(contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.Equal(contents[i], read[i]);
        }

        [Fact]
        public void TestAppendLinesToNewFileAndReadLinesWithUTF8Encoding()
        {
            const string file = "./testdata/Text.txt";
            var contents = new[] { "This is a test", "Monkey🐒" };

            context.CakeContext.FileAppendLines(file, Encoding.UTF8, contents);

            var read = context.CakeContext.FileReadLines(file, Encoding.UTF8);

            Assert.Equal(contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.Equal(contents[i], read[i]);
        }

        [Fact]
        public void TestAppendLinesToExistingFileAndReadLinesWithUTF8Encoding()
        {
            const string file = "./testdata/Text.txt";
            var contents = new[] { "This is a test", "Monkey🐒" };
            var contents1 = new[] { contents[0] };
            var contents2 = new[] { contents[1] };

            context.CakeContext.FileAppendLines(file, Encoding.UTF8, contents1);
            context.CakeContext.FileAppendLines(file, Encoding.UTF8, contents2);

            var read = context.CakeContext.FileReadLines(file, Encoding.UTF8);

            Assert.Equal(contents.Length, read.Length);

            for (int i = 0; i < read.Length; i++)
                Assert.Equal(contents[i], read[i]);
        }

        [Fact]
        public void FindTextInFilesGlob()
        {
            SetupFiles();

            var files = context.CakeContext.FindTextInFiles ("./testdata/*.txt", "Monkey");

            Assert.NotNull (files);
            Assert.Single(files);
        }

        [Fact]
        public void FindTextInFiles()
        {
            SetupFiles();

            var files = context.CakeContext.Globber.GetFiles ("./testdata/*.txt");

            var monkeyFiles = context.CakeContext.FindTextInFiles (files, "Monkey");

            Assert.NotNull (monkeyFiles);
            Assert.Single (monkeyFiles);
        }

        [Fact]
        public void FindTextInFilesWithUTF8Encoding()
        {
            SetupFilesUTF8();

            var files = context.CakeContext.FindTextInFiles("./testdata/*-utf8.txt", Encoding.UTF8, "Monkey🐒");

            Assert.NotNull(files);
            Assert.Single(files);
        }

        [Fact]
        public void FindRegexInFiles()
        {
            SetupFiles();

            var files = context.CakeContext.FindRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}");

            Assert.NotNull (files);
            Assert.Single (files);
        }

        [Fact]
        public void FindRegexInFilesWithUTF8Encoding()
        {
            SetupFilesUTF8();

            var files = context.CakeContext.FindRegexInFiles("./testdata/*-utf8.txt", Encoding.UTF8, @"\s{1}Monkey🐒\s{1,}");

            Assert.NotNull(files);
            Assert.Single(files);
        }

        [Fact]
        public void ReplaceTextInFiles()
        {
            SetupFiles();

            var files = context.CakeContext.ReplaceTextInFiles ("./testdata/*.txt", "Monkey", "Tamarin");

            Assert.NotNull (files);
            Assert.Single (files);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.Equal (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        [Fact]
        public void ReplaceTextInFilesWithUTF8Encoding()
        {
            SetupFilesUTF8();

            var files = context.CakeContext.ReplaceTextInFiles("./testdata/*.txt", Encoding.UTF8, "Monkey🐒", "Tamarin");

            Assert.NotNull(files);
            Assert.Single(files);

            foreach (var f in files)
            {
                var contents = context.CakeContext.FileReadText(f, Encoding.UTF8);
                Assert.Equal(string.Format(PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        [Fact]
        public void ReplaceRegexInFiles()
        {
            SetupFiles();

            var files = context.CakeContext.ReplaceRegexInFiles ("./testdata/*.txt", @"\s{1}Monkey\s{1,}", " Tamarin ");

            Assert.NotNull (files);
            Assert.Single(files);

            foreach (var f in files) {
                var contents = context.CakeContext.FileReadText (f);
                Assert.Equal (string.Format (PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        [Fact]
        public void ReplaceRegexInFilesWithUTF8Encoding()
        {
            SetupFilesUTF8();

            var files = context.CakeContext.ReplaceRegexInFiles("./testdata/*-utf8.txt", Encoding.UTF8, @"\s{1}Monkey🐒\s{1,}", " Tamarin ");

            Assert.NotNull(files);
            Assert.Single(files);

            foreach (var f in files)
            {
                var contents = context.CakeContext.FileReadText(f, Encoding.UTF8);
                Assert.Equal(string.Format(PATTERN_FILE_BASE_VALUE, "Tamarin"), contents);
            }
        }

        public const string GROUPS_FILE = "./testdata/Groups.txt";
        public const string GROUPS_FILE_UTF8 = "./testdata/Groups-utf8.txt";
        public const string GROUPS_FILE_CONTENT = "Hello World! This is A quick Test to Capture multiple Groups.";
        public const string GROUPS_FILE_CONTENT_UTF8 = "🐒Hello 🐒World! 🐒This is A quick 🐒Test to 🐒Capture multiple 🐒Groups.";
        public const string GROUPS_PATTERN = "([A-Z])(\\w+)";
        public const string GROUPS_PATTERN_UTF8 = "(🐒[A-Z])(\\w+)";

        [Fact]
        public void FindRegexMatchesGroupsInFile()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchesGroups = context.CakeContext.FindRegexMatchesGroupsInFile (GROUPS_FILE, GROUPS_PATTERN, RegexOptions.None);

            Assert.NotNull (matchesGroups);
            Assert.Equal (6, matchesGroups.Count);

            foreach (var g in matchesGroups)
                Assert.Equal (3, g.Count);
        }

        [Fact]
        public void FindRegexMatchesGroupsInFileWithUTF8Encoding()
        {
            context.CakeContext.FileWriteText(GROUPS_FILE_UTF8, GROUPS_FILE_CONTENT_UTF8);

            var matchesGroups = context.CakeContext.FindRegexMatchesGroupsInFile(GROUPS_FILE_UTF8, GROUPS_PATTERN_UTF8, RegexOptions.None);

            Assert.NotNull(matchesGroups);
            Assert.Equal(6, matchesGroups.Count);

            foreach (var g in matchesGroups)
                Assert.Equal(3, g.Count);
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
        public void FindRegexMatchGroupsInFileWithUTF8Encoding()
        {
            context.CakeContext.FileWriteText(GROUPS_FILE_UTF8, GROUPS_FILE_CONTENT_UTF8);

            var matchGroups = context.CakeContext.FindRegexMatchGroupsInFile(GROUPS_FILE_UTF8, GROUPS_PATTERN_UTF8, RegexOptions.None);

            Assert.NotNull(matchGroups);
            Assert.Equal(3, matchGroups.Count);
        }

        [Fact]
        public void FindRegexMatchGroupInFile()
        {
            context.CakeContext.FileWriteText (GROUPS_FILE, GROUPS_FILE_CONTENT);

            var matchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 2, RegexOptions.None);
            var invalidMatchGroup = context.CakeContext.FindRegexMatchGroupInFile (GROUPS_FILE, GROUPS_PATTERN, 8, RegexOptions.None);

            Assert.NotNull (matchGroup);
            Assert.Null (invalidMatchGroup);
            Assert.Equal ("ello", matchGroup.Value);
        }

        [Fact]
        public void FindRegexMatchGroupInFileWithUTF8Encoding()
        {
            context.CakeContext.FileWriteText(GROUPS_FILE_UTF8, GROUPS_FILE_CONTENT_UTF8);

            var matchGroup = context.CakeContext.FindRegexMatchGroupInFile(GROUPS_FILE_UTF8, GROUPS_PATTERN_UTF8, 2, RegexOptions.None);
            var invalidMatchGroup = context.CakeContext.FindRegexMatchGroupInFile(GROUPS_FILE_UTF8, GROUPS_PATTERN_UTF8, 8, RegexOptions.None);

            Assert.NotNull(matchGroup);
            Assert.Null(invalidMatchGroup);
            Assert.Equal("ello", matchGroup.Value);
        }

        public const string PATTERN_FILE_BASE_VALUE = "The {0} makes great software.\nThis is not a surprise.";

        void SetupFiles()
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
                context.CakeContext.FileWriteText(
                    string.Format("./testdata/{0}-utf8.txt", i),
                    Encoding.UTF8,
                    string.Format(PATTERN_FILE_BASE_VALUE, i == 2 ? "Monkey🐒" : "Ape🦧"));
            }
        }
    }
}


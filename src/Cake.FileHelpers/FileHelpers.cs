using Cake.Core.Annotations;
using Cake.Core;
using Cake.Core.IO;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cake.FileHelpers
{
    /// <summary>
    /// File helper aliases.
    /// A set of aliases for <see href="http://cakebuild.net">Cake Build</see> to help with simple File operations such as Reading, Writing and Replacing text.
    /// </summary>
    [CakeAliasCategory("File Helpers")]
    public static class FileHelperAliases
    {
        /// <summary>
        /// Reads all text from a file
        /// </summary>
        /// <returns>The file's text.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to read.</param>
        [CakeMethodAlias]
        public static string FileReadText (this ICakeContext context, FilePath file)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            return File.ReadAllText (filename);
        }

        /// <summary>
        /// Reads all text from a file
        /// </summary>
        /// <returns>The file's text.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        [CakeMethodAlias]
        public static string FileReadText(this ICakeContext context, FilePath file, Encoding encoding)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            return File.ReadAllText(filename, encoding);
        }

        /// <summary>
        /// Reads all lines from a file
        /// </summary>
        /// <returns>The file's text lines.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to read.</param>
        [CakeMethodAlias]
        public static string[] FileReadLines (this ICakeContext context, FilePath file)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            return File.ReadAllLines (filename);
        }

        /// <summary>
        /// Reads all lines from a file
        /// </summary>
        /// <returns>The file's text lines.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to read.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        [CakeMethodAlias]
        public static string[] FileReadLines(this ICakeContext context, FilePath file, Encoding encoding)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            return File.ReadAllLines(filename, encoding);
        }

        /// <summary>
        /// Writes all text to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to write to.</param>
        /// <param name="text">The text to write.</param>
        [CakeMethodAlias]
        public static void FileWriteText (this ICakeContext context, FilePath file, string text)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            File.WriteAllText (filename, text);
        }

        /// <summary>
        /// Writes all text to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to write to.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="text">The text to write.</param>
        [CakeMethodAlias]
        public static void FileWriteText(this ICakeContext context, FilePath file, Encoding encoding, string text)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            File.WriteAllText(filename, text, encoding);
        }

        /// <summary>
        /// Writes all text lines to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to write to.</param>
        /// <param name="lines">The text lines to write.</param>
        [CakeMethodAlias]
        public static void FileWriteLines (this ICakeContext context, FilePath file, string[] lines)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            File.WriteAllLines (filename, lines);
        }

        /// <summary>
        /// Writes all text lines to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to write to.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="lines">The text lines to write.</param>
        [CakeMethodAlias]
        public static void FileWriteLines(this ICakeContext context, FilePath file, Encoding encoding, string[] lines)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            File.WriteAllLines(filename, lines, encoding);
        }

        /// <summary>
        /// Appends all text to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to append text to.</param>
        /// <param name="text">The text to append.</param>
        [CakeMethodAlias]
        public static void FileAppendText (this ICakeContext context, FilePath file, string text)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            File.AppendAllText (filename, text);
        }

        /// <summary>
        /// Appends all text to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to append text to.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="text">The text to append.</param>
        [CakeMethodAlias]
        public static void FileAppendText(this ICakeContext context, FilePath file, Encoding encoding, string text)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            File.AppendAllText(filename, text, encoding);
        }

        /// <summary>
        /// Appends all text lines to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to append text to.</param>
        /// <param name="lines">The text lines to append.</param>
        [CakeMethodAlias]
        public static void FileAppendLines (this ICakeContext context, FilePath file, string[] lines)
        {
            var filename = file.MakeAbsolute (context.Environment).FullPath;

            File.AppendAllLines (filename, lines);
        }

        /// <summary>
        /// Appends all text lines to a file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to append text to.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="lines">The text lines to append.</param>
        [CakeMethodAlias]
        public static void FileAppendLines(this ICakeContext context, FilePath file, Encoding encoding, string[] lines)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            File.AppendAllLines(filename, lines, encoding);
        }

        /// <summary>
        /// Sets the last write time of a file to the current time
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to touch</param>
        [CakeMethodAlias]
        public static void FileTouch(this ICakeContext context, FilePath file)
        {
            var filename = file.MakeAbsolute(context.Environment).FullPath;

            File.SetLastWriteTimeUtc(filename, System.DateTime.UtcNow);
        }

        /// <summary>
        /// Replaces the text in files matched by the given globber pattern
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="findText">The text to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceTextInFiles (this ICakeContext context, string globberPattern, string findText, string replaceText)
        {
            var files = context.Globber.GetFiles (globberPattern);

            var results = new ConcurrentBag<FilePath> ();

            Parallel.ForEach (files, f => {
                var contents = FileReadText (context, f);

                if (contents.Contains (findText)) {
                    contents = contents.Replace (findText, replaceText);
                    FileWriteText (context, f, contents);

                    results.Add (f);
                }
            });

            return results.ToArray ();
        }

        /// <summary>
        /// Replaces the text in files matched by the given globber pattern
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="findText">The text to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceTextInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string findText, string replaceText)
        {
            var files = context.Globber.GetFiles(globberPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach(files, f => {
                var contents = FileReadText(context, f, encoding);

                if (contents.Contains(findText))
                {
                    contents = contents.Replace(findText, replaceText);
                    FileWriteText(context, f, encoding, contents);

                    results.Add(f);
                }
            });

            return results.ToArray();
        }

        /// <summary>
        /// Replaces the regex pattern in files matched by the given globber pattern.
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceRegexInFiles (this ICakeContext context, string globberPattern, string rxFindPattern, string replaceText)
        {
            return ReplaceRegexInFiles (context, globberPattern, rxFindPattern, replaceText, RegexOptions.None);
        }

        /// <summary>
        /// Replaces the regex pattern in files matched by the given globber pattern.
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceRegexInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string rxFindPattern, string replaceText)
        {
            return ReplaceRegexInFiles(context, globberPattern, encoding, rxFindPattern, replaceText, RegexOptions.None);
        }

        /// <summary>
        /// Replaces the regex pattern in files matched by the given globber pattern.
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        /// <param name="rxOptions">The regular expression options to use.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceRegexInFiles (this ICakeContext context, string globberPattern, string rxFindPattern, string replaceText, RegexOptions rxOptions)
        {
            var rx = new Regex (rxFindPattern, rxOptions);
            var files = context.Globber.GetFiles (globberPattern);

            var results = new ConcurrentBag<FilePath> ();

            Parallel.ForEach (files, f => {
                var contents = FileReadText (context, f);
                if (rx.IsMatch (contents)) {
                    contents = rx.Replace (contents, replaceText);
                    FileWriteText (context, f, contents);
                    results.Add (f);
                }
            });

            return results.ToArray ();
        }

        /// <summary>
        /// Replaces the regex pattern in files matched by the given globber pattern.
        /// </summary>
        /// <returns>The files that had text replaced in them.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="replaceText">The replacement text.</param>
        /// <param name="rxOptions">The regular expression options to use.</param>
        [CakeMethodAlias]
        public static FilePath[] ReplaceRegexInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string rxFindPattern, string replaceText, RegexOptions rxOptions)
        {
            var rx = new Regex(rxFindPattern, rxOptions);
            var files = context.Globber.GetFiles(globberPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach(files, f =>
            {
                var contents = FileReadText(context, f, encoding);
                if (rx.IsMatch(contents))
                {
                    contents = rx.Replace(contents, replaceText);
                    FileWriteText(context, f, encoding, contents);
                    results.Add(f);
                }
            });

            return results.ToArray();
        }

        /// <summary>
        /// Finds files with regular expression pattern in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        [CakeMethodAlias]
        public static FilePath[] FindRegexInFiles (this ICakeContext context, string globberPattern, string rxFindPattern)
        {
            return FindRegexInFiles (context, globberPattern, rxFindPattern, RegexOptions.None);
        }

        /// <summary>
        /// Finds files with regular expression pattern in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        [CakeMethodAlias]
        public static FilePath[] FindRegexInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string rxFindPattern)
        {
            return FindRegexInFiles(context, globberPattern, encoding, rxFindPattern, RegexOptions.None);
        }

        /// <summary>
        /// Finds files with regular expression pattern in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="rxOptions">The regular expression options to use.</param>
        [CakeMethodAlias]
        public static FilePath[] FindRegexInFiles (this ICakeContext context, string globberPattern, string rxFindPattern, RegexOptions rxOptions)
        {
            var rx = new Regex (rxFindPattern, rxOptions);
            var files = context.Globber.GetFiles (globberPattern);

            var results = new ConcurrentBag<FilePath> ();

            Parallel.ForEach (files, f => {
                var contents = FileReadText (context, f);
                if (rx.IsMatch (contents))
                    results.Add (f);
            });

            return results.ToArray ();
        }

        /// <summary>
        /// Finds files with regular expression pattern in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regular expression to find.</param>
        /// <param name="rxOptions">The regular expression options to use.</param>
        [CakeMethodAlias]
        public static FilePath[] FindRegexInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string rxFindPattern, RegexOptions rxOptions)
        {
            var rx = new Regex(rxFindPattern, rxOptions);
            var files = context.Globber.GetFiles(globberPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach(files, f =>
            {
                var contents = FileReadText(context, f, encoding);
                if (rx.IsMatch(contents))
                    results.Add(f);
            });

            return results.ToArray();
        }

        /// <summary>
        /// Finds files with the given text in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to find text in.</param>
        /// <param name="substring">The substring to find.</param>
        [CakeMethodAlias]
        public static FilePath[] FindTextInFiles (this ICakeContext context, string globberPattern, string substring)
        {
            var files = context.Globber.GetFiles (globberPattern);
            return FindTextInFiles(context, files, substring);
        }

        /// <summary>
        /// Finds files with the given text in files matching the given globber pattern.
        /// </summary>
        /// <returns>The files which match the regular expression and globber pattern.</returns>
        /// <param name="context">The context.</param>
        /// <param name="globberPattern">The globber pattern to match files to replace text in.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="findPattern">The regular expression to find.</param>
        [CakeMethodAlias]
        public static FilePath[] FindTextInFiles(this ICakeContext context, string globberPattern, Encoding encoding, string findPattern)
        {
            var files = context.Globber.GetFiles(globberPattern);

            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach(files, f =>
            {
                var contents = FileReadText(context, f, encoding);
                if (contents.Contains(findPattern))
                    results.Add(f);
            });

            return results.ToArray();
        }

        /// <summary>
        /// Finds files with the given text in files matching the given collection of files.
        /// </summary>
        /// <returns>The files which match the regular expession in the files.</returns>
        /// <param name="context">The context.</param>
        /// <param name="files">The files to find text in.</param>
        /// <param name="substring">The substring to find.</param>
        [CakeMethodAlias]
        public static FilePath[] FindTextInFiles(this ICakeContext context, IEnumerable<FilePath> files, string substring)
        {
            var results = new ConcurrentBag<FilePath>();

            Parallel.ForEach(files, f => {
                var contents = FileReadText(context, f);
                if (contents.Contains(substring))
                    results.Add(f);
            });

            return results.ToArray();
        }

        /// <summary>
        /// Finds the regex matches in a text file.
        /// </summary>
        /// <returns>The regex matches in file.</returns>
        /// <param name="context">Context.</param>
        /// <param name="file">The text file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static List<string> FindRegexMatchesInFile (this ICakeContext context, FilePath file, string rxFindPattern, RegexOptions rxOptions)
        {
            if (!context.FileSystem.Exist (file))
                return null;

            var rx = new Regex (rxFindPattern, rxOptions);
            var contents = FileReadText (context, file);

            var values = new List<string> ();

            var matches = rx.Matches (contents);
            foreach (Match m in matches)
                if (m.Success && m.Value != null)
                    values.Add (m.Value);

            return values;
        }

        /// <summary>
        /// Finds the regex matches in a text file.
        /// </summary>
        /// <returns>The regex matches in file.</returns>
        /// <param name="context">Context.</param>
        /// <param name="file">The text file.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static List<string> FindRegexMatchesInFile(this ICakeContext context, FilePath file, Encoding encoding, string rxFindPattern, RegexOptions rxOptions)
        {
            if (!context.FileSystem.Exist(file))
                return null;

            var rx = new Regex(rxFindPattern, rxOptions);
            var contents = FileReadText(context, file, encoding);

            var values = new List<string>();

            var matches = rx.Matches(contents);
            foreach (Match m in matches)
                if (m.Success && m.Value != null)
                    values.Add(m.Value);

            return values;
        }

        /// <summary>
        /// Finds the first regex match in a textfile.
        /// </summary>
        /// <returns>The first regex match in the file.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static string FindRegexMatchInFile (this ICakeContext context, FilePath file, string rxFindPattern, RegexOptions rxOptions)
        {
            var values = FindRegexMatchesInFile (context, file, rxFindPattern, rxOptions);

            if (values != null)
                return values.FirstOrDefault ();

            return null;
        }

        /// <summary>
        /// Finds the first regex match in a textfile.
        /// </summary>
        /// <returns>The first regex match in the file.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The encoding to use for the file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static string FindRegexMatchInFile(this ICakeContext context, FilePath file, Encoding encoding, string rxFindPattern, RegexOptions rxOptions)
        {
            var values = FindRegexMatchesInFile(context, file, encoding, rxFindPattern, rxOptions);

            if (values != null)
                return values.FirstOrDefault();

            return null;
        }

        /// <summary>
        /// Finds regex matches in a file and returns all match groups.
        /// </summary>
        /// <returns>The matches with their groups.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static List<List<Group>> FindRegexMatchesGroupsInFile (this ICakeContext context, FilePath file, string rxFindPattern, RegexOptions rxOptions)
        {
            if (!context.FileSystem.Exist (file))
                return null;

            var rx = new Regex (rxFindPattern, rxOptions);
            var contents = FileReadText (context, file);

            var values = new List<List<Group>> ();

            var matches = rx.Matches (contents);
            foreach (Match m in matches)
                if (m.Success)
                    values.Add (m.Groups.Cast<Group> ().ToList ());

            return values;
        }

        /// <summary>
        /// Finds the first regex match in a file and returns all match groups.
        /// </summary>
        /// <returns>The match groups.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static List<Group> FindRegexMatchGroupsInFile (this ICakeContext context, FilePath file, string rxFindPattern, RegexOptions rxOptions)
        {
            var groups = FindRegexMatchesGroupsInFile (context, file, rxFindPattern, rxOptions);

            return groups?.FirstOrDefault ();
        }

        /// <summary>
        /// Finds the first regex match in a file and returns a specific match group.
        /// </summary>
        /// <returns>The matches with their groups.</returns>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <param name="rxFindPattern">The regex pattern to search for.</param>
        /// <param name="groupIndex">The specific match group.</param>
        /// <param name="rxOptions">The regex options.</param>
        [CakeMethodAlias]
        public static Group FindRegexMatchGroupInFile (this ICakeContext context, FilePath file, string rxFindPattern, int groupIndex, RegexOptions rxOptions)
        {
            var matchesGroups = FindRegexMatchesGroupsInFile (context, file, rxFindPattern, rxOptions);
            var matchGroups = matchesGroups?.FirstOrDefault ();

            if (matchGroups != null && matchGroups.Count > groupIndex)
                return matchGroups[groupIndex];

            return null;
        }
    }
}


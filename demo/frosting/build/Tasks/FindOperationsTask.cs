using System;
using System.Text.RegularExpressions;
using Cake.Common.Diagnostics;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Find-Operations")]
    [IsDependentOn(typeof(TouchTask))]
    public sealed class FindOperationsTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var samples = context.WorkDir.CombineWithFilePath("sample*.txt").FullPath;

            var byText = context.FindTextInFiles(samples, "alpha");
            AssertThat(byText.Length == 2, "FindTextInFiles 'alpha': expected 2 files, got " + byText.Length);
            context.Information("FindTextInFiles 'alpha' OK ({0} files)", byText.Length);

            var byRegex = context.FindRegexInFiles(samples, @"\d+");
            AssertThat(byRegex.Length == 1, "FindRegexInFiles digits: expected 1 file (sample.txt has '42'), got " + byRegex.Length);
            context.Information("FindRegexInFiles digits OK ({0} file)", byRegex.Length);

            var single = context.FindRegexMatchInFile(context.SampleFile, @"\d+", RegexOptions.None);
            AssertThat(single == "1", "FindRegexMatchInFile: expected first digit run '1' (from 'line 1'), got " + (single ?? "null"));
            context.Information("FindRegexMatchInFile digits OK ('{0}')", single);

            var many = context.FindRegexMatchesInFile(context.SampleFile, @"alpha", RegexOptions.None);
            AssertThat(many != null && many.Count == 2, $"FindRegexMatchesInFile 'alpha': expected 2 matches, got {many?.Count ?? 0}");
            context.Information("FindRegexMatchesInFile 'alpha' OK ({0} matches)", many.Count);

            var group = context.FindRegexMatchGroupInFile(context.SampleFile, @"gamma (\d+)", 1, RegexOptions.None);
            AssertThat(group != null && group.Value == "42", "FindRegexMatchGroupInFile: expected group '42', got " + (group?.Value ?? "null"));
            context.Information("FindRegexMatchGroupInFile OK ('{0}')", group.Value);

            var groups = context.FindRegexMatchGroupsInFile(context.SampleFile, @"(alpha) (line)", RegexOptions.None);
            AssertThat(groups != null && groups.Count == 3, $"FindRegexMatchGroupsInFile: expected 3 groups (full + 2 captures), got {groups?.Count ?? 0}");
            context.Information("FindRegexMatchGroupsInFile OK ({0} groups)", groups.Count);

            var allGroups = context.FindRegexMatchesGroupsInFile(context.SampleFile, @"(alpha) (line)", RegexOptions.None);
            AssertThat(allGroups != null && allGroups.Count == 2, $"FindRegexMatchesGroupsInFile: expected 2 matches, got {allGroups?.Count ?? 0}");
            context.Information("FindRegexMatchesGroupsInFile OK ({0} matches)", allGroups.Count);
        }

        private static void AssertThat(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion failed: " + message);
            }
        }
    }
}

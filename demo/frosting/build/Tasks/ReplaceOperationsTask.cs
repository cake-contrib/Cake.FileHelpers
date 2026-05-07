using System;
using Cake.Common.Diagnostics;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Replace-Operations")]
    [IsDependentOn(typeof(FindOperationsTask))]
    public sealed class ReplaceOperationsTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var samples = context.WorkDir.CombineWithFilePath("sample*.txt").FullPath;

            var changed = context.ReplaceTextInFiles(samples, "alpha", "ALPHA");
            AssertThat(changed.Length == 2, "ReplaceTextInFiles 'alpha'->'ALPHA': expected 2 changed files, got " + changed.Length);

            var afterText = context.FileReadText(context.SampleFile);
            AssertThat(
                afterText.Contains("ALPHA") && !afterText.Contains("alpha"),
                "ReplaceTextInFiles: replacement not applied to sample.txt");
            context.Information("ReplaceTextInFiles OK ({0} files modified)", changed.Length);

            var changedRx = context.ReplaceRegexInFiles(samples, @"beta", "BETA");
            AssertThat(changedRx.Length == 1, "ReplaceRegexInFiles 'beta'->'BETA': expected 1 changed file (only sample.txt has 'beta'), got " + changedRx.Length);
            var afterRegex = context.FileReadText(context.SampleFile);
            AssertThat(afterRegex.Contains("BETA"), "ReplaceRegexInFiles: replacement not applied");
            context.Information("ReplaceRegexInFiles OK ({0} files modified)", changedRx.Length);
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

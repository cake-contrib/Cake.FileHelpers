using System;
using Cake.Common.Diagnostics;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Read-Operations")]
    [IsDependentOn(typeof(SetupTask))]
    public sealed class ReadOperationsTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var lines = context.FileReadLines(context.SampleFile);
            AssertThat(lines.Length == 4, "FileReadLines: expected 4 lines, got " + lines.Length);
            AssertThat(lines[0] == "alpha line 1", "FileReadLines: first line mismatch");
            AssertThat(lines[3] == "alpha line 4", "FileReadLines: last line mismatch");
            context.Information("FileReadLines OK ({0} lines)", lines.Length);

            var text = context.FileReadText(context.SampleFile);
            AssertThat(text.Contains("gamma 42 delta"), "FileReadText: expected content not found");
            context.Information("FileReadText OK ({0} chars)", text.Length);
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

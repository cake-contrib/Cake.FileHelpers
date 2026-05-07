using System;
using Cake.Common.Diagnostics;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Append-Operations")]
    [IsDependentOn(typeof(WriteOperationsTask))]
    public sealed class AppendOperationsTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var beforeLines = context.FileReadLines(context.WrittenFile);

            context.FileAppendText(context.WrittenFile, "appended text\n");
            context.FileAppendLines(context.WrittenFile, new[] { "appended line 1", "appended line 2" });
            var afterLines = context.FileReadLines(context.WrittenFile);

            AssertThat(
                afterLines.Length == beforeLines.Length + 3,
                $"Append: expected {beforeLines.Length + 3} lines, got {afterLines.Length}");
            AssertThat(
                afterLines[afterLines.Length - 1] == "appended line 2",
                "Append: last line mismatch");
            context.Information("FileAppendText + FileAppendLines OK ({0} lines after append)", afterLines.Length);
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

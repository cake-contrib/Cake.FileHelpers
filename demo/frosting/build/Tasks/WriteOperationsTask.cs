using System;
using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Write-Operations")]
    [IsDependentOn(typeof(ReadOperationsTask))]
    public sealed class WriteOperationsTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.FileWriteText(context.WrittenFile, "first text\n");
            var afterText = File.ReadAllText(context.MakeAbsolute(context.WrittenFile).FullPath);
            AssertThat(afterText == "first text\n", "FileWriteText: content mismatch");
            context.Information("FileWriteText OK");

            context.FileWriteLines(context.WrittenFile, new[] { "line A", "line B", "line C" });
            var afterLines = context.FileReadLines(context.WrittenFile);
            AssertThat(afterLines.Length == 3, "FileWriteLines: expected 3 lines, got " + afterLines.Length);
            AssertThat(afterLines[1] == "line B", "FileWriteLines: line 2 mismatch");
            context.Information("FileWriteLines OK (overwrote with {0} lines)", afterLines.Length);
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

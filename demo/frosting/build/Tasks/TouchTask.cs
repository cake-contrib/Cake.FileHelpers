using System;
using System.IO;
using System.Threading;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Touch")]
    [IsDependentOn(typeof(AppendOperationsTask))]
    public sealed class TouchTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            // FileTouch updates the timestamps of an existing file (it does NOT create).
            var touchPath = context.MakeAbsolute(context.TouchFile).FullPath;
            File.WriteAllText(touchPath, string.Empty);
            var beforeUtc = File.GetLastWriteTimeUtc(touchPath);
            Thread.Sleep(10);
            context.FileTouch(context.TouchFile);
            var afterUtc = File.GetLastWriteTimeUtc(touchPath);

            AssertThat(
                afterUtc > beforeUtc,
                $"FileTouch: timestamp did not advance (before={beforeUtc:o}, after={afterUtc:o})");
            context.Information("FileTouch OK (advanced from {0:o} to {1:o})", beforeUtc, afterUtc);
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

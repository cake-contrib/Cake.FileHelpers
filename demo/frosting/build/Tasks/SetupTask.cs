using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Setup")]
    public sealed class SetupTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (context.DirectoryExists(context.WorkDir))
            {
                context.DeleteDirectory(
                    context.WorkDir,
                    new DeleteDirectorySettings { Recursive = true });
            }

            context.EnsureDirectoryExists(context.WorkDir);

            File.WriteAllLines(
                context.MakeAbsolute(context.SampleFile).FullPath,
                new[] { "alpha line 1", "beta line 2", "gamma 42 delta", "alpha line 4" });
            File.WriteAllText(
                context.MakeAbsolute(context.SampleFile2).FullPath,
                "second file with the word alpha");

            context.Information("Setup complete.");
        }
    }
}

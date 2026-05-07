using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Cleanup")]
    [IsDependentOn(typeof(ReplaceOperationsTask))]
    public sealed class CleanupTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (context.DirectoryExists(context.WorkDir))
            {
                context.DeleteDirectory(
                    context.WorkDir,
                    new DeleteDirectorySettings { Recursive = true });
            }

            context.Information("Cleanup complete.");
        }
    }
}

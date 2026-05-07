using Build.Tasks;
using Cake.Frosting;

namespace Build
{
    [TaskName("Default")]
    [IsDependentOn(typeof(CleanupTask))]
    public sealed class DefaultTask : FrostingTask
    {
    }
}

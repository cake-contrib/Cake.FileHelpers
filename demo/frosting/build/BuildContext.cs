using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    public class BuildContext : FrostingContext
    {
        public DirectoryPath WorkDir { get; } = "./BuildArtifacts/temp/test-filehelpers-frosting";

        public FilePath SampleFile => WorkDir.CombineWithFilePath("sample.txt");

        public FilePath SampleFile2 => WorkDir.CombineWithFilePath("sample2.txt");

        public FilePath WrittenFile => WorkDir.CombineWithFilePath("written.txt");

        public FilePath TouchFile => WorkDir.CombineWithFilePath("touched.txt");

        public BuildContext(ICakeContext context)
            : base(context)
        {
        }
    }
}

using System;
using Cake.Core.IO;
using Cake.Core;
using System.Collections.Generic;

namespace Cake.Xamarin.Tests.Fakes
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;

        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath (
                System.IO.Path.GetFullPath (AppDomain.CurrentDomain.BaseDirectory));
            
            var fileSystem = new FileSystem ();
            var environment = new CakeEnvironment ();
            var globber = new Globber (fileSystem, environment);
            log = new FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var toolResolvers = new List<IToolResolver> ();
            var registry = new WindowsRegistry ();

            var nugetToolResolver = new Cake.Core.IO.NuGet.NuGetToolResolver (fileSystem, environment, globber);
            toolResolvers.Add (nugetToolResolver);

            context = new CakeContext (fileSystem, environment, globber, log, args, processRunner, toolResolvers, registry);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }
            
        public ICakeContext CakeContext {
            get { return context; }
        }

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, log.Messages);
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Messages)
                Console.WriteLine (m);
        }
    }
}


#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: Context.Environment.WorkingDirectory,
                            title: "Cake.FileHelpers",
                            repositoryOwner: "cake-contrib",
                            repositoryName: "Cake.FileHelpers",
                            appVeyorAccountName: "cakecontrib",
                            shouldRunDotNetCorePack: true,
                            shouldRunDupFinder: false,
                            shouldRunInspectCode: false,
                            shouldRunCodecov: false,
                            shouldPostToSlack: false,
                            shouldRunGitVersion: true
                             );

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                                BuildParameters.RootDirectoryPath + "/Cake.FileHelpers.Tests/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* -[FakeItEasy]*",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");
Build.RunDotNetCore();

#tool nuget:?package=NUnit.ConsoleRunner

var sln = "./Cake.FileHelpers.sln";
var nuspec = "./Cake.FileHelpers.nuspec";
var version = Argument ("version", "1.0.0.0");
var target = Argument ("target", "build");
var configuration = Argument("configuration", EnvironmentVariable ("CONFIGURATION") ?? "Release");

Task ("build").Does (() =>
{
	NuGetRestore (sln);
	DotNetBuild (sln, c => c.Configuration = configuration);
});

Task ("package").IsDependentOn("build").Does (() =>
{
	EnsureDirectoryExists ("./output/");

	NuGetPack (nuspec, new NuGetPackSettings {
		OutputDirectory = "./output/",
		Version = version,
	});
});

Task ("clean").Does (() =>
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

Task("test").IsDependentOn("package").Does(() =>
{
	NUnit3("./**/bin/"+ configuration + "/*.Tests.dll");
});

Task ("Default")
	.IsDependentOn ("test");

RunTarget (target);

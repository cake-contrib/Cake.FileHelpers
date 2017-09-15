#tool nuget:?package=NUnit.ConsoleRunner

var sln = "./Cake.FileHelpers.sln";
var nuspec = "./Cake.FileHelpers.nuspec";
var nugetVersion = Argument ("nuget_version", EnvironmentVariable ("NUGET_VERSION") ?? "9.9.9");
var target = Argument ("target", "build");
var configuration = Argument("configuration", EnvironmentVariable ("CONFIGURATION") ?? "Release");

Task ("build").Does (() =>
{
	MSBuild ("./Cake.FileHelpers.sln", c => c.Targets.Add ("restore"));
	MSBuild ("./Cake.FileHelpers.sln", c => c.Configuration = configuration);
});

Task ("package").IsDependentOn("build").Does (() =>
{
	MSBuild ("./Cake.FileHelpers/Cake.FileHelpers.csproj", c => {
		c.Configuration = configuration;
		c.Targets.Add ("pack");
		c.Properties.Add ("PackageVersion", new List<string> { nugetVersion });
		c.Properties.Add ("IncludeSymbols", new List<string> { "true" });
	});
});

Task ("clean").Does (() =>
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");
});

Task("test").IsDependentOn("package").Does(() =>
{
	MSBuild ("./Cake.FileHelpers.Tests/Cake.FileHelpers.Tests.csproj", c => c.Configuration = configuration);
	NUnit3("./**/bin/"+ configuration + "/**/*.Tests.dll");
});

Task ("Default")
	.IsDependentOn ("test");

RunTarget (target);

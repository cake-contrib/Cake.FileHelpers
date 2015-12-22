#tool nuget:?package=NUnit.Runners&version=2.6.4

var sln = "./Cake.FileHelpers.sln";
var nuspec = "./Cake.FileHelpers.nuspec";
var version = Argument ("version", "1.0.0.0");
var target = Argument ("target", "lib");
var configuration = Argument("configuration", "Release");

Task ("lib").Does (() =>
{
	NuGetRestore (sln);

	DotNetBuild (sln, c => c.Configuration = configuration);
});

Task ("nuget").IsDependentOn ("unit-tests").Does (() =>
{
	CreateDirectory ("./nupkg/");

	NuGetPack (nuspec, new NuGetPackSettings {
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./nupkg/",
		Version = version,
		// NuGet messes up path on mac, so let's add ./ in front again
		BasePath = "././",
	});
});

Task ("push").IsDependentOn ("nuget").Does (() =>
{
	// Get the newest (by last write time) to publish
	var newestNupkg = GetFiles ("nupkg/*.nupkg")
		.OrderBy (f => new System.IO.FileInfo (f.FullPath).LastWriteTimeUtc)
		.LastOrDefault ();

	var apiKey = TransformTextFile ("./.nugetapikey").ToString ();

	NuGetPush (newestNupkg, new NuGetPushSettings {
		Verbosity = NuGetVerbosity.Detailed,
		ApiKey = apiKey
	});
});

Task ("clean").Does (() =>
{
	CleanDirectories ("./**/bin");
	CleanDirectories ("./**/obj");

	CleanDirectories ("./**/Components");
	CleanDirectories ("./**/tools");

	DeleteFiles ("./**/*.apk");
});

Task("unit-tests").IsDependentOn("lib").Does(() =>
{
	NUnit("./**/bin/"+ configuration + "/*.Tests.dll");
});

Task ("Default")
	.IsDependentOn ("nuget");

RunTarget (target);
var sln = "./Cake.FileHelpers.sln";
var nuspec = "./Cake.FileHelpers.nuspec";

var target = Argument ("target", "lib");

Task ("lib").Does (() => 
{
	NuGetRestore (sln);

	DotNetBuild (sln, c => c.Configuration = "Release");
});

Task ("nuget").IsDependentOn ("lib").Does (() => 
{
	CreateDirectory ("./nupkg/");

	NuGetPack (nuspec, new NuGetPackSettings { 
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./nupkg/",
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

RunTarget (target);
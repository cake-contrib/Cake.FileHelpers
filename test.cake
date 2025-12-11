#reference "BuildArtifacts/temp/_PublishedLibraries/Cake.FileHelpers/net9.0/Cake.FileHelpers.dll"

using Microsoft.VisualBasic;

Task("Default")
    .IsDependentOn("ReadLines");

Task("ReadLines")
    .Does(() =>
{
    var headerRow = FileReadLines(File("./README.md"))
        .FirstOrDefault();
    Information(headerRow);
});

RunTarget("Default");
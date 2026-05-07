#!/usr/bin/env dotnet
#:sdk Cake.Sdk@6.1.1
#:project ../../src/Cake.FileHelpers/Cake.FileHelpers.csproj

// Cake SDK consumer demo for Cake.FileHelpers. Runs as a file-based
// .NET program (introduced in .NET 10) using the Cake.Sdk
// directives. The #:project directive above lets the SDK build the
// addin from source rather than referencing a published nupkg.
//
// To run locally:
//   cd demo/sdk
//   dotnet cake.cs
//
// Mirrors the alias surface exercised by demo/script/filehelpers.cake
// and demo/frosting/, against a temp working directory under
// demo/sdk/BuildArtifacts/ (gitignored). Each task asserts the
// expected outcome and throws on mismatch — the script fails
// (non-zero exit) if any alias misbehaves.

using Cake.FileHelpers;

var workDir = Directory("./BuildArtifacts/temp/test-filehelpers-sdk");
var sampleFile = workDir + File("sample.txt");
var sampleFile2 = workDir + File("sample2.txt");

Task("Default")
    .IsDependentOn("Setup")
    .IsDependentOn("Read-Operations")
    .IsDependentOn("Write-Operations")
    .IsDependentOn("Append-Operations")
    .IsDependentOn("Touch")
    .IsDependentOn("Find-Operations")
    .IsDependentOn("Replace-Operations")
    .IsDependentOn("Cleanup");

Task("Setup")
    .Does(() =>
{
    if (DirectoryExists(workDir))
    {
        DeleteDirectory(workDir, new DeleteDirectorySettings { Recursive = true });
    }

    EnsureDirectoryExists(workDir);
    System.IO.File.WriteAllLines(
        sampleFile.Path.FullPath,
        new[] { "alpha line 1", "beta line 2", "gamma 42 delta", "alpha line 4" });
    System.IO.File.WriteAllText(sampleFile2.Path.FullPath, "second file with the word alpha");
    Information("Setup complete.");
});

Task("Read-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var lines = FileReadLines(sampleFile);
    AssertThat(lines.Length == 4, "FileReadLines: expected 4 lines, got " + lines.Length);
    AssertThat(lines[0] == "alpha line 1", "FileReadLines: first line mismatch");
    AssertThat(lines[3] == "alpha line 4", "FileReadLines: last line mismatch");
    Information("FileReadLines OK ({0} lines)", lines.Length);

    var text = FileReadText(sampleFile);
    AssertThat(text.Contains("gamma 42 delta"), "FileReadText: expected content not found");
    Information("FileReadText OK ({0} chars)", text.Length);
});

Task("Write-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var writeFile = workDir + File("written.txt");

    FileWriteText(writeFile, "first text\n");
    var afterText = System.IO.File.ReadAllText(writeFile.Path.FullPath);
    AssertThat(afterText == "first text\n", "FileWriteText: content mismatch");
    Information("FileWriteText OK");

    FileWriteLines(writeFile, new[] { "line A", "line B", "line C" });
    var afterLines = FileReadLines(writeFile);
    AssertThat(afterLines.Length == 3, "FileWriteLines: expected 3 lines, got " + afterLines.Length);
    AssertThat(afterLines[1] == "line B", "FileWriteLines: line 2 mismatch");
    Information("FileWriteLines OK (overwrote with {0} lines)", afterLines.Length);
});

Task("Append-Operations")
    .IsDependentOn("Write-Operations")
    .Does(() =>
{
    var writeFile = workDir + File("written.txt");
    var beforeLines = FileReadLines(writeFile);

    FileAppendText(writeFile, "appended text\n");
    FileAppendLines(writeFile, new[] { "appended line 1", "appended line 2" });
    var afterLines = FileReadLines(writeFile);

    AssertThat(afterLines.Length == beforeLines.Length + 3,
        $"Append: expected {beforeLines.Length + 3} lines, got {afterLines.Length}");
    AssertThat(afterLines[afterLines.Length - 1] == "appended line 2",
        "Append: last line mismatch");
    Information("FileAppendText + FileAppendLines OK ({0} lines after append)", afterLines.Length);
});

Task("Touch")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var touchFile = workDir + File("touched.txt");
    System.IO.File.WriteAllText(touchFile.Path.FullPath, string.Empty);
    var beforeUtc = System.IO.File.GetLastWriteTimeUtc(touchFile.Path.FullPath);
    System.Threading.Thread.Sleep(10);
    FileTouch(touchFile);
    var afterUtc = System.IO.File.GetLastWriteTimeUtc(touchFile.Path.FullPath);

    AssertThat(afterUtc > beforeUtc,
        $"FileTouch: timestamp did not advance (before={beforeUtc:o}, after={afterUtc:o})");
    Information("FileTouch OK (advanced from {0:o} to {1:o})", beforeUtc, afterUtc);
});

Task("Find-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var byText = FindTextInFiles(workDir + File("sample*.txt"), "alpha");
    AssertThat(byText.Length == 2, "FindTextInFiles 'alpha': expected 2 files, got " + byText.Length);
    Information("FindTextInFiles 'alpha' OK ({0} files)", byText.Length);

    var byRegex = FindRegexInFiles(workDir + File("sample*.txt"), @"\d+");
    AssertThat(byRegex.Length == 1, "FindRegexInFiles digits: expected 1 file (sample.txt has '42'), got " + byRegex.Length);
    Information("FindRegexInFiles digits OK ({0} file)", byRegex.Length);

    var single = FindRegexMatchInFile(sampleFile, @"\d+", System.Text.RegularExpressions.RegexOptions.None);
    AssertThat(single == "1", "FindRegexMatchInFile: expected first digit run '1' (from 'line 1'), got " + (single ?? "null"));
    Information("FindRegexMatchInFile digits OK ('{0}')", single);

    var many = FindRegexMatchesInFile(sampleFile, @"alpha", System.Text.RegularExpressions.RegexOptions.None);
    AssertThat(many != null && many.Count == 2, $"FindRegexMatchesInFile 'alpha': expected 2 matches, got {many?.Count ?? 0}");
    Information("FindRegexMatchesInFile 'alpha' OK ({0} matches)", many.Count);

    var group = FindRegexMatchGroupInFile(sampleFile, @"gamma (\d+)", 1, System.Text.RegularExpressions.RegexOptions.None);
    AssertThat(group != null && group.Value == "42", "FindRegexMatchGroupInFile: expected group '42', got " + (group?.Value ?? "null"));
    Information("FindRegexMatchGroupInFile OK ('{0}')", group.Value);

    var groups = FindRegexMatchGroupsInFile(sampleFile, @"(alpha) (line)", System.Text.RegularExpressions.RegexOptions.None);
    AssertThat(groups != null && groups.Count == 3, $"FindRegexMatchGroupsInFile: expected 3 groups (full + 2 captures), got {groups?.Count ?? 0}");
    Information("FindRegexMatchGroupsInFile OK ({0} groups)", groups.Count);

    var allGroups = FindRegexMatchesGroupsInFile(sampleFile, @"(alpha) (line)", System.Text.RegularExpressions.RegexOptions.None);
    AssertThat(allGroups != null && allGroups.Count == 2, $"FindRegexMatchesGroupsInFile: expected 2 matches, got {allGroups?.Count ?? 0}");
    Information("FindRegexMatchesGroupsInFile OK ({0} matches)", allGroups.Count);
});

Task("Replace-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var changed = ReplaceTextInFiles(workDir + File("sample*.txt"), "alpha", "ALPHA");
    AssertThat(changed.Length == 2, "ReplaceTextInFiles 'alpha'->'ALPHA': expected 2 changed files, got " + changed.Length);

    var afterText = FileReadText(sampleFile);
    AssertThat(afterText.Contains("ALPHA") && !afterText.Contains("alpha"), "ReplaceTextInFiles: replacement not applied to sample.txt");
    Information("ReplaceTextInFiles OK ({0} files modified)", changed.Length);

    var changedRx = ReplaceRegexInFiles(workDir + File("sample*.txt"), @"beta", "BETA");
    AssertThat(changedRx.Length == 1, "ReplaceRegexInFiles 'beta'->'BETA': expected 1 changed file (only sample.txt has 'beta'), got " + changedRx.Length);
    var afterRegex = FileReadText(sampleFile);
    AssertThat(afterRegex.Contains("BETA"), "ReplaceRegexInFiles: replacement not applied");
    Information("ReplaceRegexInFiles OK ({0} files modified)", changedRx.Length);
});

Task("Cleanup")
    .IsDependentOn("Replace-Operations")
    .IsDependentOn("Append-Operations")
    .IsDependentOn("Touch")
    .IsDependentOn("Find-Operations")
    .Does(() =>
{
    if (DirectoryExists(workDir))
    {
        DeleteDirectory(workDir, new DeleteDirectorySettings { Recursive = true });
    }

    Information("Cleanup complete.");
});

RunTarget("Default");

// ----- Helpers (must come AFTER top-level statements per CS8803) -----

static void AssertThat(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception("Assertion failed: " + message);
    }
}

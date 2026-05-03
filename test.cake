#reference "BuildArtifacts/temp/_PublishedLibraries/Cake.FileHelpers/net9.0/Cake.FileHelpers.dll"

// Self-contained exercise of Cake.FileHelpers aliases against a temp
// working directory. Creates files, reads/writes/appends/replaces, then
// cleans up. Runs without any external state or credentials.

var workDir = Directory("./BuildArtifacts/temp/test-filehelpers");
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
    Information("FileReadLines -> {0} line(s); first: \"{1}\"", lines.Length, lines.FirstOrDefault());

    var text = FileReadText(sampleFile);
    Information("FileReadText -> {0} char(s)", text.Length);
});

Task("Write-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var writeFile = workDir + File("written.txt");
    FileWriteText(writeFile, "first text\n");
    Information("FileWriteText -> {0} char(s) on disk", System.IO.File.ReadAllText(writeFile.Path.FullPath).Length);

    FileWriteLines(writeFile, new[] { "line A", "line B", "line C" });
    var afterWrite = FileReadLines(writeFile);
    Information("FileWriteLines -> {0} line(s) on disk", afterWrite.Length);
});

Task("Append-Operations")
    .IsDependentOn("Write-Operations")
    .Does(() =>
{
    var writeFile = workDir + File("written.txt");
    FileAppendText(writeFile, "appended text\n");
    FileAppendLines(writeFile, new[] { "appended line 1", "appended line 2" });
    var lines = FileReadLines(writeFile);
    Information("After append, file has {0} line(s)", lines.Length);
});

Task("Touch")
    .IsDependentOn("Setup")
    .Does(() =>
{
    // FileTouch updates the timestamps of an existing file (it does NOT create)
    var touchFile = workDir + File("touched.txt");
    System.IO.File.WriteAllText(touchFile.Path.FullPath, string.Empty);
    var beforeUtc = System.IO.File.GetLastWriteTimeUtc(touchFile.Path.FullPath);
    System.Threading.Thread.Sleep(10);
    FileTouch(touchFile);
    var afterUtc = System.IO.File.GetLastWriteTimeUtc(touchFile.Path.FullPath);
    Information("FileTouch -> last write {0} -> {1} (advanced: {2})",
        beforeUtc.ToString("o"), afterUtc.ToString("o"), afterUtc > beforeUtc);
});

Task("Find-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var byText = FindTextInFiles(workDir + File("*.txt"), "alpha");
    Information("FindTextInFiles 'alpha' -> {0} file(s)", byText.Length);

    var byRegex = FindRegexInFiles(workDir + File("*.txt"), @"\d+");
    Information("FindRegexInFiles digits -> {0} file(s)", byRegex.Length);

    var single = FindRegexMatchInFile(sampleFile, @"\d+", System.Text.RegularExpressions.RegexOptions.None);
    Information("FindRegexMatchInFile digits -> \"{0}\"", single ?? "(null)");

    var many = FindRegexMatchesInFile(sampleFile, @"alpha", System.Text.RegularExpressions.RegexOptions.None);
    Information("FindRegexMatchesInFile 'alpha' -> {0} match(es)", many?.Count ?? 0);

    var group = FindRegexMatchGroupInFile(sampleFile, @"gamma (\d+)", 1, System.Text.RegularExpressions.RegexOptions.None);
    Information("FindRegexMatchGroupInFile -> \"{0}\"", group?.Value ?? "(null)");

    var groups = FindRegexMatchGroupsInFile(sampleFile, @"(alpha) (\w+)", System.Text.RegularExpressions.RegexOptions.None);
    Information("FindRegexMatchGroupsInFile -> {0} group(s)", groups?.Count ?? 0);

    var allGroups = FindRegexMatchesGroupsInFile(sampleFile, @"(alpha) (\w+)", System.Text.RegularExpressions.RegexOptions.None);
    Information("FindRegexMatchesGroupsInFile -> {0} match(es)", allGroups?.Count ?? 0);
});

Task("Replace-Operations")
    .IsDependentOn("Setup")
    .Does(() =>
{
    var changed = ReplaceTextInFiles(workDir + File("*.txt"), "alpha", "ALPHA");
    Information("ReplaceTextInFiles 'alpha'->'ALPHA' -> changed {0} file(s)", changed.Length);

    var changedRx = ReplaceRegexInFiles(workDir + File("*.txt"), @"beta", "BETA");
    Information("ReplaceRegexInFiles 'beta'->'BETA' -> changed {0} file(s)", changedRx.Length);
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

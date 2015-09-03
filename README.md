# Cake.FileHelpers
A set of aliases for http://cakebuild.net to help with simple File operations such as Reading, Writing and Replacing text.


You can easily reference Cake.Xamarin directly in your build script via a cake addin:

```csharp
#addin "Cake.FileHelpers"
```

The following Aliases are available:

### FileReadText
Reads all text from a file
```csharp
string FileReadText (FilePath projectFile)
```

### FileReadLines
Reads all lines of text from a file
```csharp
string[] FileReadLines (FilePath projectFile)
```

### FileWriteText
Writes all text to a file (overwrites it if it already exists)
```csharp
void FileWriteText (FilePath projectFile, string text)
```

### FileWriteLines
Writes all text lines to a file (overwrites it if it already exists)
```csharp
void FileWriteText (FilePath projectFile, string[] lines)
```

### FileAppendText
Appends all text to a file (creates it if it does not already exist)
```csharp
void FileAppendText (FilePath projectFile, string text)
```

### FileAppendLines
Appends all text lines to a file (creates it if it does not already exist)
```csharp
void FileAppendLines (FilePath projectFile, string[] lines)
```


### ReplaceTextInFiles
Replaces `findText` with `replaceText` in files matching the `globberPattern`.  Returns the files that had text replaced in them.
```csharp
FilePath[] ReplaceTextInFiles (string globberPattern, string findText, string replaceText)
```

### ReplaceRegexInFiles
Replaces `rxFindPattern` with `replaceText` in files matching the `globberPattern`.  Returns the files that had text replaced in them.
```csharp
FilePath[] ReplaceRegexInFiles (string globberPattern, string rxFindPattern, string replaceText)
```

### ReplaceRegexInFiles
Replaces `rxFindPattern` with `replaceText` in files matching the `globberPattern`.  Returns the files that had text replaced in them.
```csharp
FilePath[] ReplaceRegexInFiles (string globberPattern, string rxFindPattern, string replaceText, RegexOptions rxOptions)
```


### FindRegexInFiles
Finds files with contents matching `rxFindPattern` in files matching the `globberPattern`.  Returns the files found.
```csharp
FilePath[] FindRegexInFiles (string globberPattern, string rxFindPattern)
```

### FindRegexInFiles
Finds files with contents matching `rxFindPattern` in files matching the `globberPattern`.  Returns the files found.
```csharp
FilePath[] FindRegexInFiles (string globberPattern, string rxFindPattern, RegexOptions rxOptions)
```

### FindTextInFiles
Finds files with contents matching `findPattern` in files matching the `globberPattern`.  Returns the files found.
```csharp
FilePath[] FindTextInFiles (string globberPattern, string findPattern)
```
        


## Apache License 2.0
Apache Cake.FileHelpers Copyright 2015. The Apache Software Foundation This product includes software developed at The Apache Software Foundation (http://www.apache.org/).
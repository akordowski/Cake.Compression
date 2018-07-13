#load nuget:https://www.myget.org/F/arkord/api/v2?package=Cake.Baker

var twitterMessage = Messages.DefaultMessage + "\n#cakebuild #nuget #csharp #dotnet";

Task("CreateImage")
    .IsDependeeOf("Image")
    .Does(() =>
    {
        var pathTarget = Build.Paths.Directories.Image.Combine("lib/net461");

        CleanDirectory(pathTarget);
        CopyFiles(Build.Paths.Files.License.ToString(), Build.Paths.Directories.Image);
        CopyFiles(Build.Paths.Directories.PublishedLibraries + "/Cake.Compression/Cake.Compression.*", pathTarget);
    });

Build
    .SetParameters(
        "Cake.Compression",
        "akordowski")
    .SetMessages(
        twitterMessage: twitterMessage)
    .Run();
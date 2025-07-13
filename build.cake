#load nuget:https://www.myget.org/F/arkord/api/v2?package=Cake.Baker

var twitterMessage = Messages.DefaultMessage + "\n#cakebuild #nuget #csharp #dotnet";

Task("CreateImage")
    .IsDependeeOf("Image")
    .Does(() =>
    {
        var pathTarget = Build.Paths.Directories.Image.Combine("lib/net8.0");
        var pathSource = string.Format("{0}/{1}/**/{1}.*", Build.Paths.Directories.PublishedLibraries, Build.Parameters.Title);
        var files = GetFiles(pathSource).Where(fileSystemInfo =>
        {
            var fullPath = fileSystemInfo.FullPath;
            return fullPath.EndsWith(".dll") || fullPath.EndsWith(".pdb") || fullPath.EndsWith(".xml");
        });

        CleanDirectory(pathTarget);
        CopyFiles(Build.Paths.Files.License.ToString(), Build.Paths.Directories.Image);
        CopyFiles(files, pathTarget);
    });

Build
    .SetParameters(
        "Cake.Compression",
        "akordowski",
        shouldPublishToNuGet:true,
        shouldPublishToGitHub:true,
        shouldPostToTwitter:true)
    .SetMessages(
        twitterMessage: twitterMessage)
    .Run();
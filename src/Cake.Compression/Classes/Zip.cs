using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Cake.Compression.Classes;

/// <summary>
/// Provides a <see cref="Zip"/> class.
/// </summary>
public sealed class Zip : CompressionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Zip"/> class.
    /// </summary>
    public Zip(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        ICakeLog log)
        : base(fileSystem, environment, log)
    {
    }

    /// <summary>
    /// Create a Zip archive of the specified files.
    /// </summary>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <param name="level">The compression level (1-9).</param>
    public override void Compress(
        DirectoryPath rootPath,
        FilePath outputPath,
        IEnumerable<FilePath> filePaths,
        int level)
    {
        ArgumentNullException.ThrowIfNull(rootPath, nameof(rootPath));
        ArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));
        ArgumentNullException.ThrowIfNull(filePaths, nameof(filePaths));
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 1, nameof(level));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(level, 9, nameof(level));

        // Make root path and output file path absolute.
        rootPath = rootPath.MakeAbsolute(Environment);
        outputPath = outputPath.MakeAbsolute(Environment);

        // Get the output file.
        var outputFile = FileSystem.GetFile(outputPath);

        // Open up a stream to the output file.
        Log.Verbose("Creating Zip file: {0}", outputPath.FullPath);

        using (var outputStream = outputFile.Open(FileMode.Create, FileAccess.Write, FileShare.None))
        using (var zipOutputStream = new ZipOutputStream(outputStream))
        {
            zipOutputStream.SetLevel(level);

            foreach (var inputPath in filePaths)
            {
                var absoluteInputPath = inputPath.MakeAbsolute(Environment);
                var file = FileSystem.GetFile(absoluteInputPath);

                using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Get the relative filename to the rootPath.
                    var relativeFilePath = GetRelativeFilePath(rootPath, absoluteInputPath);
                    Log.Verbose("Compressing file {0}", absoluteInputPath);

                    var entryName = relativeFilePath.FullPath;
                    entryName = ZipEntry.CleanName(entryName);

                    // Create the tar archive entry.
                    var entry = new ZipEntry(entryName)
                    {
                        DateTime = File.GetLastWriteTime(absoluteInputPath.FullPath),
                        Size = inputStream.Length
                    };

                    zipOutputStream.PutNextEntry(entry);
                    inputStream.CopyTo(zipOutputStream);
                    zipOutputStream.CloseEntry();
                }
            }
        }

        Log.Verbose("Zip file successfully created: {0}", outputPath.FullPath);
    }

    /// <summary>
    /// Decompress the specified Zip file.
    /// </summary>
    /// <param name="filePath">Zip file to decompress.</param>
    /// <param name="outputPath">Output path to decompress into.</param>
    public override void Decompress(FilePath filePath, DirectoryPath outputPath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        ArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));

        // Make root path and output file path absolute.
        filePath = filePath.MakeAbsolute(Environment);
        outputPath = outputPath.MakeAbsolute(Environment);

        var file = FileSystem.GetFile(filePath);

        Log.Verbose("Decompress Zip file {0} to {1}", filePath.FullPath, outputPath.FullPath);

        using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var zipFile = new ZipFile(inputStream))
        {
            foreach (ZipEntry zipEntry in zipFile)
            {
                if (!zipEntry.IsFile)
                {
                    continue; // Ignore directories
                }

                var entryFileName = zipEntry.Name;

                var buffer = new byte[4096]; // 4K is optimum
                var zipStream = zipFile.GetInputStream(zipEntry);

                var fullZipToPath = System.IO.Path.Combine(outputPath.FullPath, entryFileName);
                var directoryName = System.IO.Path.GetDirectoryName(fullZipToPath)!;

                if (directoryName.Length > 0)
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (var streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
        }
    }
}

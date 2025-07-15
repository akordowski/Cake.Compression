using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using System.Text;

namespace Cake.Compression.Classes;

/// <summary>
/// Provides a <see cref="BZip2"/> class.
/// </summary>
public sealed class BZip2 : CompressionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BZip2"/> class.
    /// </summary>
    public BZip2(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        ICakeLog log)
        : base(fileSystem, environment, log)
    {
    }

    /// <summary>
    /// Create a BZip2 Tar archive of the specified files.
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
        Log.Verbose("Creating BZip2 file: {0}", outputPath.FullPath);

        using (var outputStream = outputFile.Open(FileMode.Create, FileAccess.Write, FileShare.None))
        using (var bzip2OutputStream = new BZip2OutputStream(outputStream, level))
        using (var tarOutputStream = new TarOutputStream(bzip2OutputStream, Encoding.UTF8))
        {
            foreach (var inputPath in filePaths)
            {
                var absoluteInputPath = inputPath.MakeAbsolute(Environment);
                var file = FileSystem.GetFile(absoluteInputPath);

                using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    // Get the relative filename to the rootPath.
                    var relativeFilePath = GetRelativeFilePath(rootPath, absoluteInputPath);
                    Log.Verbose("Compressing file {0}", absoluteInputPath);

                    // Create the tar archive entry.
                    var entry = TarEntry.CreateTarEntry(relativeFilePath.FullPath);
                    entry.Size = inputStream.Length;

                    tarOutputStream.PutNextEntry(entry);
                    inputStream.CopyTo(tarOutputStream);
                    tarOutputStream.CloseEntry();
                }
            }
        }

        Log.Verbose("BZip2 file successfully created: {0}", outputPath.FullPath);
    }

    /// <summary>
    /// Decompress the specified BZip2 Tar file.
    /// </summary>
    /// <param name="filePath">BZip2 file to decompress.</param>
    /// <param name="outputPath">Output path to decompress into.</param>
    public override void Decompress(FilePath filePath, DirectoryPath outputPath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        ArgumentNullException.ThrowIfNull(outputPath, nameof(outputPath));

        // Make root path and output file path absolute.
        filePath = filePath.MakeAbsolute(Environment);
        outputPath = outputPath.MakeAbsolute(Environment);

        var file = FileSystem.GetFile(filePath);

        Log.Verbose("Decompress BZip2 file {0} to {1}", filePath.FullPath, outputPath.FullPath);

        using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var bzip2InputStream = new BZip2InputStream(inputStream))
        {
            var archive = TarArchive.CreateInputTarArchive(bzip2InputStream, Encoding.UTF8);
            archive.ExtractContents(outputPath.FullPath);
            archive.Close();
        }
    }
}

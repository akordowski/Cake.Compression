using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Compression.Classes;

/// <summary>
/// Provides a <see cref="CompressionBase"/> class.
/// </summary>
public abstract class CompressionBase
{
    /// <summary>
    /// Gets the file system.
    /// </summary>
    protected IFileSystem FileSystem { get; }

    /// <summary>
    /// Gets the environment.
    /// </summary>
    protected ICakeEnvironment Environment { get; }

    /// <summary>
    /// Gets the log.
    /// </summary>
    protected ICakeLog Log { get; }

    /// <summary>
    /// Gets the string comparison.
    /// </summary>
    protected StringComparison Comparison { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompressionBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="log">The log.</param>
    protected CompressionBase(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        ICakeLog log)
    {
        ArgumentNullException.ThrowIfNull(fileSystem, nameof(fileSystem));
        ArgumentNullException.ThrowIfNull(environment, nameof(environment));
        ArgumentNullException.ThrowIfNull(log, nameof(log));

        FileSystem = fileSystem;
        Environment = environment;
        Log = log;
        Comparison = environment.Platform.IsUnix()
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;
    }

    /// <summary>
    /// Create a archive of the specified files.
    /// </summary>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <param name="level">The compression level (1-9).</param>
    public abstract void Compress(
        DirectoryPath rootPath,
        FilePath outputPath,
        IEnumerable<FilePath> filePaths,
        int level);

    /// <summary>
    /// Decompress the specified archive.
    /// </summary>
    /// <param name="filePath">Archive to decompress.</param>
    /// <param name="outputPath">Output path to decompress into.</param>
    public abstract void Decompress(FilePath filePath, DirectoryPath outputPath);

    /// <summary>
    /// Gets a relative file path.
    /// </summary>
    /// <param name="root">A directory path.</param>
    /// <param name="file">A file path.</param>
    /// <returns>Returns a relative file path.</returns>
    protected FilePath GetRelativeFilePath(DirectoryPath root, FilePath file)
    {
        if (file.FullPath.StartsWith(root.FullPath, Comparison))
        {
            return file.FullPath[(root.FullPath.Length + 1)..];
        }

        throw new CakeException($"File '{file.FullPath}' is not relative to root path '{root.FullPath}'.");
    }
}

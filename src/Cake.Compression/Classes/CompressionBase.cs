using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cake.Compression.Classes
{
    /// <summary>
    /// Provides a <see cref="CompressionBase"/> class.
    /// </summary>
    public abstract class CompressionBase
    {
        #region Protected Fields
        protected readonly IFileSystem fileSystem;
        protected readonly ICakeEnvironment environment;
        protected readonly ICakeLog log;
        protected readonly StringComparison comparison;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionBase"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        protected CompressionBase(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            Precondition.IsNotNull(fileSystem, nameof(fileSystem));
            Precondition.IsNotNull(environment, nameof(environment));
            Precondition.IsNotNull(log, nameof(log));

            this.fileSystem = fileSystem;
            this.environment = environment;
            this.log = log;
            this.comparison = environment.Platform.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create a archive of the specified files.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="level">The compression level (1-9).</param>
        public abstract void Compress(DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, int level);

        /// <summary>
        /// Uncompress the specified archive.
        /// </summary>
        /// <param name="filePath">Archive to uncompress.</param>
        /// <param name="outputPath">Output path to uncompress into.</param>
        public abstract void Uncompress(FilePath filePath, DirectoryPath outputPath);
        #endregion

        #region Protected Methods
        protected FilePath GetRelativeFilePath(DirectoryPath root, FilePath file)
        {
            if (!file.FullPath.StartsWith(root.FullPath, comparison))
            {
                const string format = "File '{0}' is not relative to root path '{1}'.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, file.FullPath, root.FullPath));
            }

            return file.FullPath.Substring(root.FullPath.Length + 1);
        }
        #endregion
    }
}
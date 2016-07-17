using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Collections.Generic;
using System.IO;

namespace Cake.Compression.Classes
{
	/// <summary>
	/// Provides a <see cref="GZip"/> class.
	/// </summary>
	public class GZip : CompressionBase
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="GZip"/> class.
		/// </summary>
		public GZip(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
			: base(fileSystem, environment, log)
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Create a GZip Tar archive of the specified files.
		/// </summary>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		public override void Compress(DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, int level)
		{
			Precondition.IsNotNull(rootPath, nameof(rootPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));
			Precondition.IsNotNull(filePaths, nameof(filePaths));
			Precondition.IsBetween(level, 1, 9, nameof(level));

			// Make root path and output file path absolute.
			rootPath = rootPath.MakeAbsolute(environment);
			outputPath = outputPath.MakeAbsolute(environment);

			// Get the output file.
			var outputFile = fileSystem.GetFile(outputPath);

			// Open up a stream to the output file.
			log.Verbose("Creating Zip file: {0}", outputPath.FullPath);

			using (var outputStream = outputFile.Open(FileMode.Create, FileAccess.Write, FileShare.None))
			using (var gzipOutputStream = new GZipOutputStream(outputStream))
			using (var tarOutputStream = new TarOutputStream(gzipOutputStream))
			{
				gzipOutputStream.SetLevel(level);

				foreach (var inputPath in filePaths)
				{
					var absoluteInputPath = inputPath.MakeAbsolute(environment);
					var file = fileSystem.GetFile(absoluteInputPath);

					using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						// Get the relative filename to the rootPath.
						var relativeFilePath = GetRelativeFilePath(rootPath, absoluteInputPath);
						log.Verbose("Compressing file {0}", absoluteInputPath);

						// Create the tar archive entry.
						TarEntry entry = TarEntry.CreateTarEntry(relativeFilePath.FullPath);
						entry.Size = inputStream.Length;

						tarOutputStream.PutNextEntry(entry);
						inputStream.CopyTo(tarOutputStream);
						tarOutputStream.CloseEntry();
					}
				}
			}

			log.Verbose("GZip file successfully created: {0}", outputPath.FullPath);
		}

		/// <summary>
		/// Uncompress the specified GZip Tar file.
		/// </summary>
		/// <param name="filePath">GZip file to uncompress.</param>
		/// <param name="outputPath">Output path to uncompress into.</param>
		public override void Uncompress(FilePath filePath, DirectoryPath outputPath)
		{
			Precondition.IsNotNull(filePath, nameof(filePath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));

			// Make root path and output file path absolute.
			filePath = filePath.MakeAbsolute(environment);
			outputPath = outputPath.MakeAbsolute(environment);

			var file = fileSystem.GetFile(filePath);

			log.Verbose("Uncompress GZip file {0} to {1}", filePath.FullPath, outputPath.FullPath);

			using (Stream inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
			using (Stream gzipInputStream = new GZipInputStream(inputStream))
			{
				TarArchive archive = TarArchive.CreateInputTarArchive(gzipInputStream);
				archive.ExtractContents(outputPath.FullPath);
				archive.Close();
			}
		}
		#endregion
	}
}
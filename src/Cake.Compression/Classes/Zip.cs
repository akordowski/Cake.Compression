using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;

namespace Cake.Compression.Classes
{
	/// <summary>
	/// Provides a <see cref="Zip"/> class.
	/// </summary>
	public class Zip : CompressionBase
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Zip"/> class.
		/// </summary>
		public Zip(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
			: base(fileSystem, environment, log)
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Create a Zip archive of the specified files.
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
			using (var zipOutputStream = new ZipOutputStream(outputStream))
			{
				zipOutputStream.SetLevel(level);

				foreach (var inputPath in filePaths)
				{
					var absoluteInputPath = inputPath.MakeAbsolute(environment);
					var file = fileSystem.GetFile(absoluteInputPath);

					using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						// Get the relative filename to the rootPath.
						var relativeFilePath = GetRelativeFilePath(rootPath, absoluteInputPath);
						log.Verbose("Compressing file {0}", absoluteInputPath);

						string entryName = relativeFilePath.FullPath;
						entryName = ZipEntry.CleanName(entryName);

						// Create the tar archive entry.
						ZipEntry entry = new ZipEntry(entryName)
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

			log.Verbose("Zip file successfully created: {0}", outputPath.FullPath);
		}

		/// <summary>
		/// Uncompress the specified Zip file.
		/// </summary>
		/// <param name="filePath">Zip file to uncompress.</param>
		/// <param name="outputPath">Output path to uncompress into.</param>
		public override void Uncompress(FilePath filePath, DirectoryPath outputPath)
		{
			Precondition.IsNotNull(filePath, nameof(filePath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));

			// Make root path and output file path absolute.
			filePath = filePath.MakeAbsolute(environment);
			outputPath = outputPath.MakeAbsolute(environment);

			var file = fileSystem.GetFile(filePath);

			log.Verbose("Uncompress Zip file {0} to {1}", filePath.FullPath, outputPath.FullPath);

			using (Stream inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
			using (ZipFile zipFile = new ZipFile(inputStream))
			{
				foreach (ZipEntry zipEntry in zipFile)
				{
					if (!zipEntry.IsFile)
					{
						continue; // Ignore directories
					}

					string entryFileName = zipEntry.Name;

					byte[] buffer = new byte[4096]; // 4K is optimum
					Stream zipStream = zipFile.GetInputStream(zipEntry);

					string fullZipToPath = System.IO.Path.Combine(outputPath.FullPath, entryFileName);
					string directoryName = System.IO.Path.GetDirectoryName(fullZipToPath);

					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}

					using (FileStream streamWriter = File.Create(fullZipToPath))
					{
						StreamUtils.Copy(zipStream, streamWriter, buffer);
					}
				}
			}
		}
		#endregion
	}
}
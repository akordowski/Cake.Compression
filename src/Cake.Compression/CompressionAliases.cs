using Cake.Common.IO;
using Cake.Compression.Classes;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Compression
{
	/// <summary>
	/// Contains functionality related to compress files.
	/// </summary>
	[CakeAliasCategory("Compression")]
	public static class CompressionAliases
	{
		#region Private Constants
		private const int Level = 6;
		#endregion

		#region BZip2
		/// <summary>
		/// Create a BZip2 Tar archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <example>
		/// <code>
		/// BZip2Compress("./publish", "publish.tar.bz2");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
		{
			Compress<BZip2>(context, rootPath, outputPath, Level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// BZip2Compress("./publish", "publish.tar.bz2", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, int level)
		{
			Compress<BZip2>(context, rootPath, outputPath, level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <example>
		/// <code>
		/// BZip2Compress("./", "xmlfiles.tar.bz2", "./*.xml");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
		{
			Compress<BZip2>(context, rootPath, outputPath, pattern, Level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// BZip2Compress("./", "xmlfiles.tar.bz2", "./*.xml", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern, int level)
		{
			Compress<BZip2>(context, rootPath, outputPath, pattern, level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// BZip2Compress("./", "cakeassemblies.tar.bz2", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
		{
			Compress<BZip2>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// BZip2Compress("./", "cakeassemblies.tar.bz2", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, int level)
		{
			Compress<BZip2>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// BZip2Compress("./", "cakebinaries.tar.bz2", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths)
		{
			Compress<BZip2>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a BZip2 Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// BZip2Compress("./", "cakebinaries.tar.bz2", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Compress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths, int level)
		{
			Compress<BZip2>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Uncompress the specified BZip2 Tar file.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="filePath">BZip2 file to uncompress.</param>
		/// <param name="outputPath">Output path to uncompress into.</param>
		/// <example>
		/// <code>
		/// ZipUncompress("Cake.tar.bz2", "./cake");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void BZip2Uncompress(this ICakeContext context, FilePath filePath, DirectoryPath outputPath)
		{
			Uncompress<BZip2>(context, filePath, outputPath);
		}
		#endregion

		#region GZip
		/// <summary>
		/// Create a GZip Tar archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <example>
		/// <code>
		/// GZipCompress("./publish", "publish.tar.gz");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
		{
			Compress<GZip>(context, rootPath, outputPath, Level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// GZipCompress("./publish", "publish.tar.gz", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, int level)
		{
			Compress<GZip>(context, rootPath, outputPath, level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <example>
		/// <code>
		/// GZipCompress("./", "xmlfiles.tar.gz", "./*.xml");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
		{
			Compress<GZip>(context, rootPath, outputPath, pattern, Level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// GZipCompress("./", "xmlfiles.tar.gz", "./*.xml", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern, int level)
		{
			Compress<GZip>(context, rootPath, outputPath, pattern, level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// GZipCompress("./", "cakeassemblies.tar.gz", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
		{
			Compress<GZip>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// GZipCompress("./", "cakeassemblies.tar.gz", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, int level)
		{
			Compress<GZip>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// GZipCompress("./", "cakebinaries.tar.gz", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths)
		{
			Compress<GZip>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a GZip Tar archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// GZipCompress("./", "cakebinaries.tar.gz", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths, int level)
		{
			Compress<GZip>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Uncompress the specified GZip Tar file.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="filePath">GZip file to uncompress.</param>
		/// <param name="outputPath">Output path to uncompress into.</param>
		/// <example>
		/// <code>
		/// ZipUncompress("Cake.tar.gz", "./cake");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void GZipUncompress(this ICakeContext context, FilePath filePath, DirectoryPath outputPath)
		{
			Uncompress<GZip>(context, filePath, outputPath);
		}
		#endregion

		#region Zip
		/// <summary>
		/// Create a Zip archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <example>
		/// <code>
		/// ZipCompress("./publish", "publish.zip");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
		{
			Compress<Zip>(context, rootPath, outputPath, Level);
		}

		/// <summary>
		/// Create a Zip archive of the specified directory.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// ZipCompress("./publish", "publish.zip", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, int level)
		{
			Compress<Zip>(context, rootPath, outputPath, level);
		}

		/// <summary>
		/// Create a Zip archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <example>
		/// <code>
		/// ZipCompress("./", "xmlfiles.zip", "./*.xml");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
		{
			Compress<Zip>(context, rootPath, outputPath, pattern, Level);
		}

		/// <summary>
		/// Create a Zip archive of the files matching the specified pattern.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="pattern">The pattern.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// ZipCompress("./", "xmlfiles.zip", "./*.xml", 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern, int level)
		{
			Compress<Zip>(context, rootPath, outputPath, pattern, level);
		}

		/// <summary>
		/// Create a Zip archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// ZipCompress("./", "cakeassemblies.zip", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
		{
			Compress<Zip>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a Zip archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = GetFiles("./**/Cake.*.dll");
		/// ZipCompress("./", "cakeassemblies.zip", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, int level)
		{
			Compress<Zip>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Create a Zip archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// ZipCompress("./", "cakebinaries.zip", files);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths)
		{
			Compress<Zip>(context, rootPath, outputPath, filePaths, Level);
		}

		/// <summary>
		/// Create a Zip archive of the specified files.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="rootPath">The root path.</param>
		/// <param name="outputPath">The output path.</param>
		/// <param name="filePaths">The file paths.</param>
		/// <param name="level">The compression level (1-9).</param>
		/// <example>
		/// <code>
		/// var files = new [] {
		///     "./src/Cake/bin/Debug/Autofac.dll",
		///     "./src/Cake/bin/Debug/Cake.Common.dll",
		///     "./src/Cake/bin/Debug/Cake.Core.dll",
		///     "./src/Cake/bin/Debug/Cake.exe"
		/// };
		/// ZipCompress("./", "cakebinaries.zip", files, 6);
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipCompress(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths, int level)
		{
			Compress<Zip>(context, rootPath, outputPath, filePaths, level);
		}

		/// <summary>
		/// Uncompress the specified Zip Tar file.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="filePath">Zip file to uncompress.</param>
		/// <param name="outputPath">Output path to uncompress into.</param>
		/// <example>
		/// <code>
		/// ZipUncompress("Cake.zip", "./cake");
		/// </code>
		/// </example>
		[CakeMethodAlias]
		public static void ZipUncompress(this ICakeContext context, FilePath filePath, DirectoryPath outputPath)
		{
			Uncompress<Zip>(context, filePath, outputPath);
		}
		#endregion

		#region Private Static Methods
		private static T CreateInstance<T>(params object[] args)
		{
			return (T)Activator.CreateInstance(typeof(T), args);
		}

		private static void Compress<T>(
			ICakeContext context,
			DirectoryPath rootPath,
			FilePath outputPath,
			int level) where T : CompressionBase
		{
			Precondition.IsNotNull(context, nameof(context));
			Precondition.IsNotNull(rootPath, nameof(rootPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));
			Precondition.IsBetween(level, 1, 9, nameof(level));

			FilePathCollection filePaths = context.GetFiles(string.Concat(rootPath, "/**/*"));
			Compress<T>(context, rootPath, outputPath, filePaths, level);
		}

		private static void Compress<T>(
			ICakeContext context,
			DirectoryPath rootPath,
			FilePath outputPath,
			string pattern,
			int level) where T : CompressionBase
		{
			Precondition.IsNotNull(context, nameof(context));
			Precondition.IsNotNull(rootPath, nameof(rootPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));
			Precondition.IsNotNullOrEmpty(pattern, nameof(pattern));
			Precondition.IsBetween(level, 1, 9, nameof(level));

			FilePathCollection filePaths = context.GetFiles(pattern);

			if (filePaths.Count == 0)
			{
				context.Log.Verbose("The provided pattern did not match any files.");
				return;
			}

			Compress<T>(context, rootPath, outputPath, filePaths, level);
		}

		private static void Compress<T>(
			ICakeContext context,
			DirectoryPath rootPath,
			FilePath outputPath,
			IEnumerable<FilePath> filePaths,
			int level) where T : CompressionBase
		{
			Precondition.IsNotNull(context, nameof(context));
			Precondition.IsNotNull(rootPath, nameof(rootPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));
			Precondition.IsNotNull(filePaths, nameof(filePaths));
			Precondition.IsBetween(level, 1, 9, nameof(level));

			T zip = CreateInstance<T>(context.FileSystem, context.Environment, context.Log);
			zip.Compress(rootPath, outputPath, filePaths, level);
		}

		private static void Compress<T>(
			ICakeContext context,
			DirectoryPath rootPath,
			FilePath outputPath,
			IEnumerable<string> filePaths,
			int level) where T : CompressionBase
		{
			Precondition.IsNotNull(context, nameof(context));
			Precondition.IsNotNull(rootPath, nameof(rootPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));
			Precondition.IsNotNull(filePaths, nameof(filePaths));
			Precondition.IsBetween(level, 1, 9, nameof(level));

			List<FilePath> paths = filePaths.Select(path => new FilePath(path)).ToList<FilePath>();
			Compress<T>(context, rootPath, outputPath, paths, level);
		}

		private static void Uncompress<T>(
			ICakeContext context,
			FilePath zipPath,
			DirectoryPath outputPath) where T : CompressionBase
		{
			Precondition.IsNotNull(context, nameof(context));
			Precondition.IsNotNull(zipPath, nameof(zipPath));
			Precondition.IsNotNull(outputPath, nameof(outputPath));

			T zip = CreateInstance<T>(context.FileSystem, context.Environment, context.Log);
			zip.Uncompress(zipPath, outputPath);
		}
		#endregion
	}
}
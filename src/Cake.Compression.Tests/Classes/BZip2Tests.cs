using Cake.Compression.Classes;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Cake.Compression.Tests.Classes
{
    [TestFixture]
    public class BZip2Test
    {
        #region Private Fields
        private readonly DirectoryPath rootPath = "/Root";
        private readonly FilePath outputPath = "/archive.tar.bz2";
        private readonly IEnumerable<FilePath> filePaths = new FilePath[] { "/Root/file.txt" };
        private readonly int level = 6;
        #endregion

        #region Constructor Tests
        [Test]
        public void Constructor_Should_Throw_If_FileSystem_Is_Null()
        {
            // Given
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();

            // Then
            Assert.That(() => new BZip2(null, environment, log),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("fileSystem"));
        }

        [Test]
        public void Constructor_Should_Throw_If_Environment_Is_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var log = Substitute.For<ICakeLog>();

            // Then
            Assert.That(() => new BZip2(fileSystem, null, log),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("environment"));
        }

        [Test]
        public void Constructor_Should_Throw_If_Log_Is_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();

            // Then
            Assert.That(() => new BZip2(fileSystem, environment, null),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("log"));
        }
        #endregion

        #region Methods Tests
        [Test]
        public void Compress_Should_Throw_If_RootPath_Is_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(null, outputPath, filePaths, level),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("rootPath"));
        }

        [Test]
        public void Compress_Should_Throw_If_OutputPath_Is_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(rootPath, null, filePaths, level),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("outputPath"));
        }

        [Test]
        public void Compress_Should_Throw_If_FilePaths_Are_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(rootPath, outputPath, null, level),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("filePaths"));
        }

        [TestCase(0)]
        [TestCase(10)]
        public void Compress_Should_Throw_If_Level_Is_Invalid(int compressLevel)
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(rootPath, outputPath, filePaths, compressLevel),
                Throws.InstanceOf<ArgumentOutOfRangeException>()
                .And.Property("ParamName").EqualTo("level"));
        }

        [Test]
        public void Compress_Should_Throw_If_File_Is_Not_Relative_To_Root()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/NotRoot/file.txt");
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(rootPath, outputPath, new FilePath[] { "/NotRoot/file.txt" }, level),
                Throws.InstanceOf<CakeException>()
                .And.Message.EqualTo("File '/NotRoot/file.txt' is not relative to root path '/Root'."));
        }

        [Test]
        public void Compress_Creates_BZip2_File()
        {
            // Given
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt");
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Compress(rootPath, outputPath, filePaths, level), Throws.Nothing);
            Assert.That(fileSystem.Exist(outputPath), Is.True);
        }

        [Test]
        public void Uncompress_Should_Throw_If_FilePath_Is_Null()
        {
            // Given
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Uncompress(null, "/Uncompress"),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("filePath"));
        }

        [Test]
        public void Uncompress_Should_Throw_If_OutputPath_Is_Null()
        {
            // Given
            var filePath = "/archive.tar.bz2";
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile(filePath);
            var log = Substitute.For<ICakeLog>();
            var zip = new BZip2(fileSystem, environment, log);

            // Then
            Assert.That(() => zip.Uncompress(filePath, null),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Property("ParamName").EqualTo("outputPath"));
        }
        #endregion
    }
}
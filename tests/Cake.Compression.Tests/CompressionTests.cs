using Cake.Compression.Classes;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using FluentAssertions;
using NSubstitute;

// ReSharper disable ObjectCreationAsStatement

namespace Cake.Compression.Tests;

public class CompressionTests
{
    private static readonly Type TypeArgumentNullException = typeof(ArgumentNullException);
    private static readonly Type TypeArgumentOutOfRangeException = typeof(ArgumentOutOfRangeException);
    private static readonly Type TypeCakeException = typeof(CakeException);

    private static readonly DirectoryPath RootPath = "/Root";
    private static readonly DirectoryPath OutputPath = "/Decompressed";
    private static readonly FilePath OutputArchivePath = "/archive.ext";
    private static readonly IEnumerable<FilePath> FilePaths = ["/Root/file.txt"];
    private const int Level = 6;

    public static TheoryData<Action, string> ConstructWithNullParameters
    {
        get
        {
            var fileSystem = Substitute.For<IFileSystem>();
            var environment = Substitute.For<ICakeEnvironment>();
            var log = Substitute.For<ICakeLog>();

            return new TheoryData<Action, string>
            {
                { () => new BZip2(null!, environment, log), "fileSystem" },
                { () => new BZip2(fileSystem, null!, log), "environment" },
                { () => new BZip2(fileSystem, environment, null!), "log" },

                { () => new GZip(null!, environment, log), "fileSystem" },
                { () => new GZip(fileSystem, null!, log), "environment" },
                { () => new GZip(fileSystem, environment, null!), "log" },

                { () => new Zip(null!, environment, log), "fileSystem" },
                { () => new Zip(fileSystem, null!, log), "environment" },
                { () => new Zip(fileSystem, environment, null!), "log" }
            };
        }
    }

    public static TheoryData<Action, string?, string?, Type> CompressWithInvalidParameters
    {
        get
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/NotRoot/file.txt");
            var log = Substitute.For<ICakeLog>();

            var bzip = new BZip2(fileSystem, environment, log);
            var gzip = new GZip(fileSystem, environment, log);
            var zip = new Zip(fileSystem, environment, log);

            FilePath bzipOutputPath = "/archive.tar.bz2";
            FilePath gzipOutputPath = "/archive.tar.gz";
            FilePath zipOutputPath = "/archive.zip";
            IEnumerable<FilePath> filePathsNotRoot = ["/NotRoot/file.txt"];

            const string message = "File '/NotRoot/file.txt' is not relative to root path '/Root'.";

            return new TheoryData<Action, string?, string?, Type>
            {
                { () => bzip.Compress(null!, bzipOutputPath, FilePaths, Level), "rootPath", null, TypeArgumentNullException },
                { () => bzip.Compress(RootPath, null!, FilePaths, Level), "outputPath", null, TypeArgumentNullException },
                { () => bzip.Compress(RootPath, bzipOutputPath, null!, Level), "filePaths", null, TypeArgumentNullException },
                { () => bzip.Compress(RootPath, bzipOutputPath, FilePaths, 0), "level", null, TypeArgumentOutOfRangeException },
                { () => bzip.Compress(RootPath, bzipOutputPath, FilePaths, 10), "level", null, TypeArgumentOutOfRangeException },
                { () => bzip.Compress(RootPath, bzipOutputPath, filePathsNotRoot, Level), null, message, TypeCakeException },

                { () => gzip.Compress(null!, gzipOutputPath, FilePaths, Level), "rootPath", null, TypeArgumentNullException },
                { () => gzip.Compress(RootPath, null!, FilePaths, Level), "outputPath", null, TypeArgumentNullException },
                { () => gzip.Compress(RootPath, gzipOutputPath, null!, Level), "filePaths", null, TypeArgumentNullException },
                { () => gzip.Compress(RootPath, gzipOutputPath, FilePaths, 0), "level", null, TypeArgumentOutOfRangeException },
                { () => gzip.Compress(RootPath, gzipOutputPath, FilePaths, 10), "level", null, TypeArgumentOutOfRangeException },
                { () => gzip.Compress(RootPath, gzipOutputPath, filePathsNotRoot, Level), null, message, TypeCakeException },

                { () => zip.Compress(null!, zipOutputPath, FilePaths, Level), "rootPath", null, TypeArgumentNullException },
                { () => zip.Compress(RootPath, null!, FilePaths, Level), "outputPath", null, TypeArgumentNullException },
                { () => zip.Compress(RootPath, zipOutputPath, null!, Level), "filePaths", null, TypeArgumentNullException },
                { () => zip.Compress(RootPath, zipOutputPath, FilePaths, 0), "level", null, TypeArgumentOutOfRangeException },
                { () => zip.Compress(RootPath, zipOutputPath, FilePaths, 10), "level", null, TypeArgumentOutOfRangeException },
                { () => zip.Compress(RootPath, zipOutputPath, filePathsNotRoot, Level), null, message, TypeCakeException },
            };
        }
    }

    public static TheoryData<Action, string> DecompressWithNullParameters
    {
        get
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var log = Substitute.For<ICakeLog>();

            var bzip = new BZip2(fileSystem, environment, log);
            var gzip = new GZip(fileSystem, environment, log);
            var zip = new Zip(fileSystem, environment, log);

            return new TheoryData<Action, string>
            {
                { () => bzip.Decompress(null!, OutputPath), "filePath" },
                { () => bzip.Decompress("/archive.tar.bz2", null!), "outputPath" },

                { () => gzip.Decompress(null!, OutputPath), "filePath" },
                { () => gzip.Decompress("/archive.tar.gz", null!), "outputPath" },

                { () => zip.Decompress(null!, OutputPath), "filePath" },
                { () => zip.Decompress("/archive.zip", null!), "outputPath" }
            };
        }
    }

    public static TheoryData<CompressionBase, FakeFileSystem> CompressionBaseInstances
    {
        get
        {
            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile("/Root/file.txt");
            var log = Substitute.For<ICakeLog>();

            return new TheoryData<CompressionBase, FakeFileSystem>
            {
                { new BZip2(fileSystem, environment, log), fileSystem },
                { new GZip(fileSystem, environment, log), fileSystem },
                { new Zip(fileSystem, environment, log), fileSystem },
            };
        }
    }

    [Theory]
    [MemberData(nameof(ConstructWithNullParameters))]
    public void Should_Throw_On_Construct_With_Null_Parameters(Action act, string parameterName)
    {
        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Theory]
    [MemberData(nameof(CompressWithInvalidParameters))]
    public void Should_Throw_On_Compress_With_Invalid_Parameters(Action act, string? parameterName, string? message, Type exceptionType)
    {
        // Assert
        if (exceptionType == TypeArgumentNullException)
        {
            act.Should().Throw<ArgumentNullException>().WithParameterName(parameterName);
        }
        else if (exceptionType == TypeArgumentOutOfRangeException)
        {
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName(parameterName);
        }
        else if (exceptionType == TypeCakeException)
        {
            act.Should().Throw<CakeException>().WithMessage(message);
        }
    }

    [Theory]
    [MemberData(nameof(DecompressWithNullParameters))]
    public void Should_Throw_On_Decompress_With_Null_Parameters(Action act, string parameterName)
    {
        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName(parameterName);
    }

    [Theory]
    [MemberData(nameof(CompressionBaseInstances))]
    public void Should_Compress_Files(CompressionBase zip, FakeFileSystem fileSystem)
    {
        // Act
        zip.Compress(RootPath, OutputArchivePath, FilePaths, Level);

        // Assert
        fileSystem.Exist(OutputArchivePath).Should().BeTrue();
    }
}

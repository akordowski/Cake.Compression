# Cake.Compression
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)
[![Build status](https://ci.appveyor.com/api/projects/status/6a3mixcwnvb1q4nn/branch/master?svg=true)](https://ci.appveyor.com/project/ArturKordowski/cake-compression/branch/master)
[![NuGet Version](https://img.shields.io/nuget/v/Cake.Compression.svg)](https://www.nuget.org/packages/Cake.Compression/)

A Cake AddIn which provides compression functionality for BZip2, GZip and Zip.

## Referencing

Cake.Compression is available as a nuget package from the package manager console:

```csharp
Install-Package Cake.Compression
```

or directly in your build script via a Cake `#addin` directive:

```csharp
#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression
```
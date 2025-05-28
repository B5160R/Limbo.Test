using System.Xml.Linq;
using Limbo.Test.Security.Models.NuGetPackageModels;
using Limbo.Test.Security.NuGetPackage.Models.ValidationModels;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace Limbo.Test.Security;

public class NuGetPackageTests {
    private readonly string[] _projectFiles;
    private readonly string _solutionDirectory;
    private PackageMetadataResource _nugetResource;

    public NuGetPackageTests() {
        // Set the solution directory to the parent of the current directory
        // This assumes that the test project is in a subdirectory of the solution directory
        _solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName
            ?? throw new DirectoryNotFoundException("Solution directory not found.");
        _projectFiles = Directory.GetFiles(_solutionDirectory, "*.csproj", SearchOption.AllDirectories);
    }

    [SetUp]
    public async Task Setup() {
        _nugetResource = await GetNugetResourceAsync();
        if (_nugetResource == null) {
            throw new Exception("Failed to retrieve NuGet resource.");
        }
    }

    [Test]
    [TestCaseSource(typeof(AllowedLicsenses), nameof(AllowedLicsenses.GetTestCases))]
    public async Task Validate_Package_Licenses_Allowed(HashSet<string> allowedLicenses) {
        //Arrange
        var restrictedNuGetPackages = new List<PackageLicenseInfo>();

        //Act
        foreach (var packageReference in GetAllPackageReferencesAsync()) {
            var metadata = await GetNuGetMetadataAsync(packageReference);
            if (metadata == null) {
                Console.WriteLine($"No metadata found for {packageReference.NuGetPackage} ({packageReference.Version}) in {packageReference.Project}");
                continue;
            }
            var license = metadata.LicenseMetadata?.License ?? metadata.Authors;

            if (!allowedLicenses.Contains(license)) {
                restrictedNuGetPackages.Add(new PackageLicenseInfo {
                    NuGetPackage = packageReference.NuGetPackage,
                    Version = packageReference.Version,
                    Project = packageReference.Project,
                    License = license
                });
            }
        }

        //Assert
        Assert.That(restrictedNuGetPackages, Is.Empty, "Restricted NuGet packages found:\n" +
            string.Join("\n", restrictedNuGetPackages.Select(p => $"- {p.NuGetPackage} ({p.Version}) in {p.Project} [License: {p.License}]")));
    }

    [Test]
    public async Task Validate_No_Package_Vulnerabilities() {
        //Arrange
        var vulnerableNuGetPackages = new List<PackageVulnerabilityInfo>();

        //Act
        foreach (var packageReference in GetAllPackageReferencesAsync()) {
            var metadata = await GetNuGetMetadataAsync(packageReference);
            if (metadata == null) {
                Console.WriteLine($"No metadata found for {packageReference.NuGetPackage} ({packageReference.Version}) in {packageReference.Project}");
                continue;
            }
            var vulnerabilities = metadata.Vulnerabilities ?? Enumerable.Empty<PackageVulnerabilityMetadata>();

            if (vulnerabilities.Any()) {
                vulnerableNuGetPackages.Add(new PackageVulnerabilityInfo {
                    NuGetPackage = packageReference.NuGetPackage,
                    Version = packageReference.Version,
                    Project = packageReference.Project,
                    Vulnerabilities = vulnerabilities.ToList()
                });
            }
        }

        //Assert
        Assert.That(vulnerableNuGetPackages, Is.Empty, "The following NuGet packages have vulnerabilities: " +
            string.Join(", ", vulnerableNuGetPackages.Select(p => $"{p.NuGetPackage} ({p.Version}) in {p.Project} with vulnerabilities {string.Join(", ", p.Vulnerabilities)}")));
    }

    [Test]
    [TestCaseSource(typeof(AllowedNuGetVersions), nameof(AllowedNuGetVersions.GetTestCases))]
    public void Validate_NuGetPackages_Has_Allowed_Versions(string nugetPackageName, string version, string projectName) {
        //Arrange
        var projectFiles = Array.Empty<string>();
        // if projectName is empty get all project files
        if (string.IsNullOrEmpty(projectName)) {
            projectFiles = _projectFiles;
        } else {
            // if projectName is not empty get only the project files that contain the projectName
            projectFiles = _projectFiles.Where(p => p.Contains(projectName)).ToArray();
        }

        var packages = new List<PackageInfo>();

        //Act
        foreach (var projectFile in projectFiles) {
            packages.AddRange(ListNuGetPackages(projectFile).ToList());
        }

        List<PackageInfo> packageInProjects = packages .Where(p => p.NuGetPackage.Equals(nugetPackageName, StringComparison.OrdinalIgnoreCase)) .ToList();

        //Assert
        Assert.Multiple(() => {
            foreach (var package in packageInProjects) {
                Assert.That(package.Version, Is.EqualTo(version), $"The package '{nugetPackageName}' in project '{package.Project}' does not have the expected version '{version}'. Actual version: '{package.Version}'.");
            }
        });
    }


    // Retrieves the NuGet metadata resource for querying package information
    private async Task<PackageMetadataResource> GetNugetResourceAsync() {
        var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        return await repository.GetResourceAsync<PackageMetadataResource>();
    }

    private IEnumerable<PackageInfo> GetAllPackageReferencesAsync() {
        var packageReferences = new List<PackageInfo>();

        foreach (var projectFile in _projectFiles) {
            packageReferences.AddRange(ListNuGetPackages(projectFile));
        }

        return packageReferences;
    }

    // Parses a .csproj file and returns all NuGet package references
    private static IEnumerable<PackageInfo> ListNuGetPackages(string projectFilePath) {
        return XDocument
            .Load(projectFilePath)
            .Descendants("PackageReference")
            .Select(packageReference => new PackageInfo {
                Project = Path.GetFileNameWithoutExtension(projectFilePath),
                NuGetPackage = packageReference.Attribute("Include")?.Value ?? string.Empty,
                Version = packageReference.Attribute("Version")?.Value ?? string.Empty
            });
    }

    // Retrieves NuGet metadata for a specific package
    private async Task<IPackageSearchMetadata> GetNuGetMetadataAsync(PackageInfo packageReference) {
        var packageIdentity = new PackageIdentity(packageReference.NuGetPackage, NuGetVersion.Parse(packageReference.Version));
        return await _nugetResource.GetMetadataAsync(packageIdentity, new SourceCacheContext(), NullLogger.Instance, default);
    }
}
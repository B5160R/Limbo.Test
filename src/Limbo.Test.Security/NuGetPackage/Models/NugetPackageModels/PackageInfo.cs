namespace Limbo.Test.Security.Models.NuGetPackageModels;

public class PackageInfo {
    public required string Project { get; set; }
    public required string NuGetPackage { get; set; }
    public required string Version { get; set; }
}
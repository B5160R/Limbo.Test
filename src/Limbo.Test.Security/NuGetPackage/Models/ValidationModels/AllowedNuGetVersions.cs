namespace Limbo.Test.Security.NuGetPackage.Models.ValidationModels;
public class AllowedNuGetVersions {
    public static object[] GetTestCases =
    {
        // new object[] { "NUnit.Analyzers", "4.4.0", "Limbo.Test.Integration" },
        new object[] { "NUnit.Analyzers", "4.4.0", "Limbo.Test.Security" },
        new object[] { "NUnit", "5.3.2", ""}
    };
}
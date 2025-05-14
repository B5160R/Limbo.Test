namespace Limbo.Test.Security.NuGetPackage.Models.ValidationModels;
public class AllowedLicsenses {
    public static IEnumerable<HashSet<string>> GetTestCases() {
        yield return new HashSet<string> {
            "MIT",
            "Microsoft",
            "Apache-2.0"
        };
    }
}
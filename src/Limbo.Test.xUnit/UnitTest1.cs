using Xunit;
using Limbo.Test.xUnit.TestHelpers;

namespace Limbo.Test.xUnit;
public class UnitTest1 {
    [Fact]
    public void TestProjectsExist() {
        var projects = TestHelper.GetProjectsToTest();
        Assert.NotEmpty(projects);

        foreach (var project in projects) {
            Assert.True(Directory.Exists(project), $"Project path does not exist: {project}");
        }
    }
}
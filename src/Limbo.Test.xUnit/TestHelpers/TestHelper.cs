using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Limbo.Test.xUnit.TestHelpers;

public static class TestHelper {
    public static WebApplicationFactory<Herningsholm.Web.Program> GetProjectProgram() {
        var projects = GetProjectsToTest();
        if (projects.Count == 0) {
            throw new InvalidOperationException("No projects found in TestConfig.json.");
        }

        string projectPath = projects[0]; // Use the first project for testing

        // Build the path to the target assembly (adjust as needed for your project structure)
        string assemblyPath = Path.Combine(projectPath, "backend", "web", "bin", "Debug", "net8.0", "Herningsholm.Web.dll");

        if (!File.Exists(assemblyPath)) {
            throw new FileNotFoundException($"Assembly not found at {assemblyPath}");
        }

        // Dynamically configure the WebApplicationFactory to use the specified assembly
        var factoryWithProgram = new WebApplicationFactory<Herningsholm.Web.Program>()
            .WithWebHostBuilder(builder => {
                builder.UseSetting("ApplicationKey", assemblyPath);
            });

        return factoryWithProgram;
    }

    public static List<string> GetProjectsToTest() {
        string configPath = Path.Combine(AppContext.BaseDirectory, "Config", "TestConfig.json");

        if (!File.Exists(configPath)) {
            throw new FileNotFoundException($"Configuration file not found at {configPath}");
        }

        string json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<TestConfig>(json);
        return config?.ProjectsToTest ?? new List<string>();
    }

    private class TestConfig {
        public List<string>? ProjectsToTest { get; set; }
    }
}
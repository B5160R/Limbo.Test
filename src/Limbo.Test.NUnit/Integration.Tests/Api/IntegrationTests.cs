using System.Net;
using Herningsholm.Features.Employees.Repository;
using Herningsholm.Web;
using NUnit.Framework;

public class IntegrationTests : IntegrationTestBase<Program> {

    [TestCase(TestName = "Root page returns 200 OK")]
    public async Task GetRootPage_ReturnsOK() {
        // arrange
        string url = "api/spa/GetData?apphost=localhost&navLevels=2&navContext=true&url=/&parts=content,site";

        // act
        var response = await GetAsync<HttpResponseMessage>(url);

        // assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    // TODO: Find a way to set reference dynamically (not like now by using...)
    [TestCase(TestName = "Can I get some Service around here?")]
    public void GetService_IsPossible() {

        // arrange
        var someService = GetService<HeEmployeesRepository>();

        // act
        var serviceTest = someService.Search(null, letter: "L", 0, 0, null);

        // assert
        Assert.That(someService, Is.Not.Null, "Service is null");
        Assert.That(someService, Is.InstanceOf<HeEmployeesRepository>(), "Service is not of type HeEmployeesRepository");

        Assert.That(serviceTest, Is.Not.Null, "Service is null");
    }

}
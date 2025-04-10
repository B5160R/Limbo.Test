// using NUnit.Framework;
// using Umbraco.Cms.Tests.Integration;
// using Umbraco.Cms.Core.Services;
// using Umbraco.Cms.Tests.Integration.Testing;
//
// [TestFixture]
// public class MyUmbracoIntegrationTests : UmbracoIntegrationT // {
//     private IContentService _contentService;
//
//     [SetUp]
//     public void SetU    {
//         _contentService = GetRequiredService<IContentService>();
//     }
//
//     [Test]
//     public void Can_Create_Conten    {
//         // Arrange
//         var contentType = _contentService.GetContentType("myContentType");
//         var content = _contentService.Create("My Content", -1, "myContentType");
//
//         // Act
//         _contentService.Save(content);
//
//         // Assert
//         Assert.IsNotNull(content.Id);
//     }
// }

using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Limbo.Test.Project.Integration;

public class TestIntegrationController : UmbracoApiController {

    private readonly IContentTypeService _contentTypeService;

    public TestIntegrationController(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }

    [HttpGet]
    [Route("api/schema/{documentTypeAlias}")]
    public IActionResult GetSchema(string documentTypeAlias) {
        var schemaGenerator = new JsonSchemaGenerator(_contentTypeService);
        var schema = schemaGenerator.GenerateSchema(documentTypeAlias);
        return Ok(schema);
    }
}

public class JsonSchemaGenerator {
    private readonly IContentTypeService _contentTypeService;

    public JsonSchemaGenerator(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }

    public JObject GenerateSchema(string documentTypeAlias) {
        // Get the document type by alias
        IContentType? contentType = _contentTypeService.Get(documentTypeAlias);
        if (contentType == null) throw new Exception($"Document type '{documentTypeAlias}' not found.");

        // Create the root schema object
        var schema = new JObject {
            ["type"] = "object",
            ["properties"] = new JObject()
        };

        // Add properties based on the document type's properties
        foreach (var property in contentType.PropertyTypes) {
            ((JObject) schema["properties"]!).Add(property.Alias, new JObject {
                ["type"] = MapUmbracoPropertyTypeToJsonType(property.PropertyEditorAlias)
            });
        }

        return schema;
    }

    private string MapUmbracoPropertyTypeToJsonType(string propertyEditorAlias) {
        // Map Umbraco property types to JSON schema types
        return propertyEditorAlias switch {
            "Umbraco.TextBox" => "string",
            "Umbraco.Integer" => "integer",
            "Umbraco.TrueFalse" => "boolean",
            "Umbraco.DateTime" => "string",
            "Umbraco.MediaPicker" => "object", // Could be expanded to include media details
            "Umbraco.MultiNodeTreePicker" => "array",
            _ => "string" // Default to string for unknown types
        };
    }
}


// WORKING INTEGRATION
// Simple copy paste in code/Features

// using Newtonsoft.Json.Linq;
// using Umbraco.Cms.Core.Services;
// using Umbraco.Cms.Core.Models;
// using Umbraco.Cms.Web.Common.Controllers;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Herningsholm.Features.TestIntegration;
//
// public class TestIntegrationController : UmbracoApiController {
//
//     private readonly IContentTypeService _contentTypeService;
//
//     public TestIntegrationController(IContentTypeService contentTypeService) {
//         _contentTypeService = contentTypeService;
//     }
//
//     [HttpGet]
//     [Route("api/schema/{documentTypeAlias}")]
//     public IActionResult GetSchema(string documentTypeAlias) {
//         var schemaGenerator = new JsonSchemaGenerator(_contentTypeService);
//         var schema = schemaGenerator.GenerateSchema(documentTypeAlias);
//         return Ok(schema);
//     }
// }
//
// public class JsonSchemaGenerator {
//     private readonly IContentTypeService _contentTypeService;
//
//     public JsonSchemaGenerator(IContentTypeService contentTypeService) {
//         _contentTypeService = contentTypeService;
//     }
//
//     public IContentType GenerateSchema(string documentTypeAlias) {
//         // Get the document type by alias
//         IContentType? contentType = _contentTypeService.Get(documentTypeAlias);
//         if (contentType == null) throw new Exception($"Document type '{documentTypeAlias}' not found.");
//         return contentType;
//
//
//     }
//
// }
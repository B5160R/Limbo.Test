using Umbraco.Cms.Core.Models;

// Record to hold property details for validation
internal record DocumentTypeDetails(string? DocumentTypeName, IPropertyType? Property, IContentType ContentType, bool IsElementType);
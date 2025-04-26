using Umbraco.Cms.Core.Models;
// Record to hold property details for validation
public record DocumentTypeDetails(string? DocumentTypeName, IPropertyType? Property, IContentType ContentType, bool IsElementType);
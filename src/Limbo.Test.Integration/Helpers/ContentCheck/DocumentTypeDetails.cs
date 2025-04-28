using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models;

public record BaseModel();
// Record to hold property details for validation
public record DocumentTypeDetails(string? DocumentTypeName, IPropertyType? Property, IContentType ContentType, bool IsElementType) : BaseModel;
// Record to hold the tokens for backoffice and SPA response
public class TokensContainer {
    public JToken? backofficeToken;
    public JToken? spaResponseToken;
}
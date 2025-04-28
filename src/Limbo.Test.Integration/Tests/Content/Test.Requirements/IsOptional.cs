// Requirement to check if a property is optional (not mandatory)
internal sealed class IsOptional : IContentRequirementBase<DocumentTypeDetails> {

    // Checks if the property is not mandatory
    public bool IsRequirementMet(DocumentTypeDetails content) {
        return content.Property != null && !content.Property.Mandatory;
    }
}
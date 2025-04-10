// Requirement to check if a property is optional (not mandatory)
internal sealed class IsOptional : IContentRequirementBase<PropertyDetails> {

    // Checks if the property is not mandatory
    public bool IsRequirementMet(PropertyDetails content) {
        return !content.Property.Mandatory;
    }
}
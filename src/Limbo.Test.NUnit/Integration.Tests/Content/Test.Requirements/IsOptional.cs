internal sealed class IsOptional : IContentRequirementBase<PropertyDetails> {
    public bool IsRequirementMet(PropertyDetails content) {
        return !content.Property.Mandatory;
    }
}
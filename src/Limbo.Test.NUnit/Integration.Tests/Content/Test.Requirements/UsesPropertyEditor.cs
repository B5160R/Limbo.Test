// Requirement to check if a property uses a specific property editor
internal sealed class UsesPropertyEditor(string _propertyEditorAlias, bool _invert = false)
    : IContentRequirementBase<PropertyDetails> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(PropertyDetails content) {
        bool shouldBe = !_invert;

        return content.Property.PropertyEditorAlias
            .Equals(_propertyEditorAlias, StringComparison.OrdinalIgnoreCase) == shouldBe;
    }
}
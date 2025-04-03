internal sealed class UsesPropertyEditor : IContentRequirementBase<PropertyDetails> {
    private readonly string _propertyEditorAlias;
    private readonly bool _invert;

    public UsesPropertyEditor(string propertyEditorAlias, bool invert = false) {
        _propertyEditorAlias = propertyEditorAlias;
        _invert = invert;
    }

    public bool IsRequirementMet(PropertyDetails content) {
        // 👇 'invert' may turn the requirement from 'should be' to 'should not be'.
        bool shouldBe = !_invert;
        return content.Property.PropertyEditorAlias.Equals(_propertyEditorAlias, StringComparison.OrdinalIgnoreCase) == shouldBe;
    }
}
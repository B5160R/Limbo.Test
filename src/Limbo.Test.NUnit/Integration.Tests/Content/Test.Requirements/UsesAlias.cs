// Requirement to check if a property uses a specific property editor
internal sealed class UsesAlias(string _alias, bool _invert = false)
    : IContentRequirementBase<DocumentTypeDetails> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(DocumentTypeDetails content) {
        bool shouldBe = !_invert;

        return content.ContentType.Alias
            .Equals(_alias, StringComparison.OrdinalIgnoreCase) == shouldBe;
    }
}
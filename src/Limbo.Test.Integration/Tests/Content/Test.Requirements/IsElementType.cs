// Requirement to check if a content type is an element type
internal sealed class IsElementType(bool _invert = false)
    : IContentRequirementBase<DocumentTypeDetails> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(DocumentTypeDetails content) {
        bool shouldBe = !_invert;

        return content.IsElementType == shouldBe;
    }
}
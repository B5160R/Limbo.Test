// Requirement to check if a property uses a specific property editor
internal sealed class IsPage(bool _invert = false)
    : IContentRequirementBase<TokensContainer> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(TokensContainer content) {
        bool shouldBe = !_invert;
        // check if the content is a page by checking to se if it starts with 'umb:'
        return content.backofficeToken?.Parent?.SelectToken("udi")?.ToString().StartsWith("umb://document/") == shouldBe;
    }
}
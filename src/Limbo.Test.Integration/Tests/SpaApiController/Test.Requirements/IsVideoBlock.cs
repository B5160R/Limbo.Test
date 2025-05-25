// Requirement to check if a property uses a specific property editor
internal sealed class IsVideoBlock(bool _invert = false)
    : IContentRequirementBase<TokensContainer> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(TokensContainer content) {
        bool shouldBe = !_invert;

        return content?.backofficeToken?.SelectToken("video") is not null == shouldBe;
    }
}
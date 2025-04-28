// Requirement to check if a property uses a specific property editor
internal sealed class IsSettingsData(bool _invert = false)
    : IContentRequirementBase<TokensContainer> {

    // Checks if the property editor alias matches the expected value
    public bool IsRequirementMet(TokensContainer content) {
        bool shouldBe = !_invert;

        return content.backofficeToken?.Parent?.SelectToken("settingsData") is null == shouldBe;
    }
}
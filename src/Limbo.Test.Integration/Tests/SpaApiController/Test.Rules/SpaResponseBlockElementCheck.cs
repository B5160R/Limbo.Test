using Newtonsoft.Json.Linq;

internal sealed class SpaResponseBlockElementCheck : SpaContentCheck<TokensContainer> {
    public SpaResponseBlockElementCheck(IReadOnlyCollection<IContentRequirementBase<TokensContainer>> requirements) : base(requirements) { }

    protected override ValueTask ValidateRulesContentAsync(TokensContainer content) {
        Assert.That(content.backofficeToken, Is.Not.Null, "Backoffice token is null");
        Assert.That(content.spaResponseToken, Is.Not.Null, "SPA response token is null");

        Assert.That(content.backofficeUdi, Is.EquivalentTo(content.spaResponseKey), $"Sub-level element with key {content.backofficeToken} not found in SPA response.");

        // Assert.That(JToken.DeepEquals(content.backofficeToken, content.spaResponseToken), Is.True, $"Sub-level element content with key {content.backofficeToken} is not equal with SPA response.");

        return ValueTask.CompletedTask;
    }
}
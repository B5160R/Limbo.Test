internal sealed class SpaResponseBlockElementCheck : ContentCheckBase<TokensContainer> {
    public SpaResponseBlockElementCheck(IReadOnlyCollection<IContentRequirementBase<TokensContainer>> requirements, string projectInitials) : base(requirements, projectInitials) { }

    protected override ValueTask ValidateRulesContentAsync(TokensContainer content) {
        Assert.That(content.backofficeToken, Is.Not.Null, "Backoffice token is null");
        Assert.That(content.spaResponseToken, Is.Not.Null, "SPA response token is null");

        Assert.That(content.backofficeToken, Is.EquivalentTo(content.spaResponseToken), $"Sub-level element with key {content.backofficeToken} not found in SPA response.");

        return ValueTask.CompletedTask;
    }
}
using Newtonsoft.Json.Linq;

internal abstract class SpaContentCheck<T>(IReadOnlyCollection<IContentRequirementBase<T>> _requirements){
    public async Task RunAssertionsAsync(IEnumerable<JToken> backOfficetokens, IEnumerable<JToken> spaResponseTokens) {
        var jsonHandler = new JsonHandler();
        var tokensContainer = new TokensContainer();

        await Task.Run(() => {
            Assert.Multiple(async () => {
                foreach (var backofficeToken in backOfficetokens) {

                    // Selects the first token from the SPA response that matches the UDI of the backoffice root token
                    var backOfficeUdi = backofficeToken.SelectToken("udi")?.ToString();
                    backOfficeUdi = backOfficeUdi?.Replace("umb://element/", string.Empty) ?? string.Empty;

                    var spaResponseToken = spaResponseTokens.FirstOrDefault(e => e.SelectToken("key")?.ToString().Replace("-", string.Empty) == backOfficeUdi);

                    var backOfficeSubLevelTokens = jsonHandler.InspectForUdi(backofficeToken);
                    var spaSubLevelElements = jsonHandler.InspectForKey(spaResponseToken);

                    int backOfficeSubLevelCount = backOfficeSubLevelTokens.Count;

                    foreach (var backOfficeSubLevelToken in backOfficeSubLevelTokens) {
                        var subLevelUdiKey = backOfficeSubLevelToken.SelectToken("udi")?.ToString() ?? string.Empty;
                        subLevelUdiKey = subLevelUdiKey.Replace("umb://element/", string.Empty);

                        tokensContainer.backofficeToken = subLevelUdiKey;

                        var isRequirementMet = await ValidateRequirementsAsync((T) (object) tokensContainer);
                        if (!isRequirementMet) continue;

                        int spaSubLevelCount = spaSubLevelElements.Count;
                        foreach (var spaSubLevelElement in spaSubLevelElements) {
                            var subLevelSpaKey = spaSubLevelElement.SelectToken("key")?.ToString() ?? string.Empty;
                            subLevelSpaKey = subLevelSpaKey.Replace("-", string.Empty);

                            if (subLevelUdiKey == subLevelSpaKey) {
                                tokensContainer.spaResponseToken = subLevelSpaKey;

                                // Validate the rules content for the matched element
                                await ValidateRulesContentAsync((T) (object) tokensContainer);

                                // Remove the matched element to avoid duplicate checks
                                spaSubLevelElements.Remove(spaSubLevelElement);
                                // Exit the loop if a match is found
                                break;
                            }

                            spaSubLevelCount--;
                            if (spaSubLevelCount == 0) {
                                Assert.Fail($"Sub-level element with key {subLevelUdiKey} not found in SPA response.");
                            }
                        }
                    }
                }
            });
        });
    }

    private ValueTask<bool> ValidateRequirementsAsync(T content) {
       return ValueTask.FromResult(_requirements.All(requirement => requirement.IsRequirementMet(content)));
    }

    protected abstract ValueTask ValidateRulesContentAsync(T content);
}
using Newtonsoft.Json.Linq;

internal abstract class SpaContentCheck<T>(IReadOnlyCollection<IContentRequirementBase<T>> _requirements) {
    public async Task RunAssertionsAsync(IEnumerable<JToken> backOfficetokens, IEnumerable<JToken> spaResponseTokens) {
        var jsonHandler = new JsonHandler();
        var tokensContainer = new TokensContainer();

        var assertionSuccesses = new List<string>();
        var assertionFailures = new List<string>();

        foreach (var backofficeToken in backOfficetokens) {
            var backOfficeUdi = backofficeToken.SelectToken("udi")?.ToString();
            backOfficeUdi = backOfficeUdi?.Replace("umb://element/", string.Empty) ?? string.Empty;

            var spaResponseToken = spaResponseTokens.FirstOrDefault(e => e.SelectToken("key")?.ToString().Replace("-", string.Empty) == backOfficeUdi);

            var backOfficeSubLevelTokens = jsonHandler.InspectForUdi(backofficeToken);
            var spaSubLevelElements = jsonHandler.InspectForKey(spaResponseToken);

            foreach (var backOfficeSubLevelToken in backOfficeSubLevelTokens) {
                var subLevelUdiKey = string.Empty;
                    subLevelUdiKey = backOfficeSubLevelToken.SelectToken("udi")?.ToString() ?? string.Empty;
                    subLevelUdiKey = subLevelUdiKey.Replace("umb://element/", string.Empty);

                if (assertionSuccesses.Contains(subLevelUdiKey) is true) {
                    Console.WriteLine($"Sub-level element with key {subLevelUdiKey} already asserted successfully.");
                    continue; // Skip if already asserted successfully
                }

                tokensContainer.backofficeToken = backOfficeSubLevelToken;
                tokensContainer.backofficeUdi = subLevelUdiKey;

                var isRequirementMet = await ValidateRequirementsAsync((T) (object) tokensContainer);
                if (!isRequirementMet) {
                    Console.WriteLine($"Requirement not met for backoffice token with UDI {subLevelUdiKey}.");
                    continue;
                }

                bool found = false;
                foreach (var spaSubLevelElement in spaSubLevelElements.ToList()) {

                    var subLevelSpaKey = string.Empty;

                    if (spaSubLevelElement.SelectToken("image") is not null) {
                        subLevelSpaKey = spaSubLevelElement.Parent?.SelectToken("key")?.ToString() ?? string.Empty;
                    }
                    else {
                        subLevelSpaKey = spaSubLevelElement.SelectToken("key")?.ToString() ?? string.Empty;
                    }

                    subLevelSpaKey = subLevelSpaKey.Replace("-", string.Empty);

                    if (subLevelUdiKey == subLevelSpaKey) {
                        assertionSuccesses.Add(subLevelUdiKey);
                        tokensContainer.spaResponseToken = spaSubLevelElement;
                        tokensContainer.spaResponseKey = subLevelSpaKey;
                        await ValidateRulesContentAsync((T) (object) tokensContainer);
                        try {
                            await ValidateRulesContentAsync((T) (object) tokensContainer);
                        } catch (AssertionException ex) {
                            assertionFailures.Add(ex.Message);
                        }
                        spaSubLevelElements.Remove(spaSubLevelElement);
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    assertionFailures.Add($"Sub-level element with key {subLevelUdiKey} not found in SPA response.");
                }
            }
        }

        Assert.Multiple(() => {
            foreach (var failure in assertionFailures) {
                Assert.Fail(failure);
            }
        });
    }

    private ValueTask<bool> ValidateRequirementsAsync(T content) {
        return ValueTask.FromResult(_requirements.All(requirement => requirement.IsRequirementMet(content)));
    }

    protected abstract ValueTask ValidateRulesContentAsync(T content);
}
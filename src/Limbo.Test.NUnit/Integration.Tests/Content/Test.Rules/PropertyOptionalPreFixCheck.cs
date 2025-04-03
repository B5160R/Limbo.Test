
internal sealed class PropertyOptionalPrefixCheck : ContentCheckBase<PropertyDetails> {
    private static readonly Dictionary<string, string> _cultureToPrefixMap = new Dictionary<string, string>() {
        ["dk-DK"] = "(Valgfrit) ",
        ["en-US"] = "(Optional) "
    };

    public PropertyOptionalPrefixCheck(IReadOnlyCollection<IContentRequirementBase<PropertyDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(PropertyDetails content) {

        var prefix = _cultureToPrefixMap[content.Culture];
        if (content.Description == null || !content.Description.StartsWith(prefix))
        {
            throw new InvalidOperationException(
            $"\"{content.Name}\" ({content.Property.Alias}) in \"{content.ContentType.Name}\" is optional");
        }

        return ValueTask.CompletedTask;
    }
}
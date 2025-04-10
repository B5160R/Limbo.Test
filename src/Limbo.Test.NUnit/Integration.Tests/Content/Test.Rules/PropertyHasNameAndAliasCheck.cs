using NUnit.Framework;

internal sealed class PropertyHasNameAndAliasCheck : ContentCheckBase<PropertyDetails> {
    public PropertyHasNameAndAliasCheck(IReadOnlyCollection<IContentRequirementBase<PropertyDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(PropertyDetails content) {

        Assert.That(content, Is.Not.Null, "Content is null");
        
        Assert.That(content.Property.Name, Is.Not.Null.Or.Empty,
            string.Format("Property name is null or empty for property with ID {0}", content.Property.Id));

        Assert.That(content.Property.Alias, Is.Not.Null.Or.Empty,
            string.Format("Property alias is null or empty for property with ID {0}", content.Property.Id));

        return ValueTask.CompletedTask;
    }
}
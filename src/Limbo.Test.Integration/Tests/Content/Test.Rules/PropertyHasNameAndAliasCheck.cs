using NUnit.Framework;

// Content check to ensure properties have valid names and aliases
internal sealed class PropertyHasNameAndAliasCheck : ContentCheckBase<DocumentTypeDetails> {
    public PropertyHasNameAndAliasCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements, string projectInitials) : base(requirements, projectInitials) { }

    protected override ValueTask DoValidateContentAsync(DocumentTypeDetails content) {
        if (content.Property is null) {
            Assert.Fail($"Property is null for content in Document Type with name: {content.DocumentTypeName}");
            return ValueTask.CompletedTask;
        }

        Assert.That(content, Is.Not.Null, "Content is null");

        Assert.That(content.Property.Name, Is.Not.Null.Or.Empty,
            string.Format("Property name is null or empty for property with ID {0}", content.Property.Id));

        Assert.That(content.Property.Alias, Is.Not.Null.Or.Empty,
            string.Format("Property alias is null or empty for property with ID {0}", content.Property.Id));

        return ValueTask.CompletedTask;
    }
}
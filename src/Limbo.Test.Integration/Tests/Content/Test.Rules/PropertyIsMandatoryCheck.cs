using NUnit.Framework;

internal sealed class PropertyIsMandatoryCheck : PropertyContentCheck<DocumentTypeDetails> {
    public PropertyIsMandatoryCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements) : base(requirements) { }

    protected override ValueTask ValidateRulesContentAsync(DocumentTypeDetails content) {
        Assert.That(content.Property?.Mandatory, Is.True,
            $"Property '{content.Property?.Name}' on document type '{content.DocumentTypeName}' should be mandatory.");
        return ValueTask.CompletedTask;
    }
}
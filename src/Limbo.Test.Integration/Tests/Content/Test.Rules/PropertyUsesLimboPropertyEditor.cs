using NUnit.Framework;

// Content check to ensure properties use Limbo or Skybrud property editors
internal sealed class PropertyUsesLimboPropertyEditorCheck : PropertyContentCheck<DocumentTypeDetails> {
    public PropertyUsesLimboPropertyEditorCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements,
        string projectInitials) : base(requirements, projectInitials) { }

    protected override ValueTask ValidateRulesContentAsync(DocumentTypeDetails content) {
        // Assert that the property editor alias starts with "Limbo" or "Skybrud"
        Assert.That(content.Property?.PropertyEditorAlias, Does.StartWith("Limbo").Or.StartWith("Skybrud"),
            $"Property '{content.Property?.Name}' on document type '{content.DocumentTypeName}' should use a Limbo property editor.");

        return ValueTask.CompletedTask;
    }
}
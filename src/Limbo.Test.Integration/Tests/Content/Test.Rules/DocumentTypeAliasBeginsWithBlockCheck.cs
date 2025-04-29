// Content check to ensure properties have valid names and aliases
internal sealed class DocumentTypeAliasBeginsWithBlockCheck : ContentCheckBase<DocumentTypeDetails> {
    public DocumentTypeAliasBeginsWithBlockCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements) : base(requirements) { }

    protected override ValueTask ValidateRulesContentAsync(DocumentTypeDetails content) {

        Assert.That(content, Is.Not.Null, "Content is null");

        Assert.That(content.ContentType.Alias, Is.Not.Null.Or.Empty,
            string.Format("Document type alias is null or empty for document type with ID {0}", content.ContentType.Id));

        Assert.That(content.ContentType.Alias, Does.StartWith("Block"),
            string.Format("Document type alias '{0}' does not start with 'Block' for document type with ID {1}", content.ContentType.Alias, content.ContentType.Id));

        return ValueTask.CompletedTask;
    }
}
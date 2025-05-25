// Content check to ensure properties have valid names and aliases
internal sealed class DocumentTypeAliasBeginsWithProjectInitialsOrModuleCheck : PropertyContentCheck<DocumentTypeDetails> {
    private readonly string _projectInitials;
    public DocumentTypeAliasBeginsWithProjectInitialsOrModuleCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements,
        string projectInitials) : base(requirements, projectInitials) {
        _projectInitials = projectInitials;
    }

    protected override ValueTask ValidateRulesContentAsync(DocumentTypeDetails content) {

        Assert.That(content, Is.Not.Null, "Content is null");

        Assert.That(content.ContentType.Alias, Is.Not.Null.Or.Empty,
            string.Format("Document type alias is null or empty for document type with ID {0}", content.ContentType.Id));

        Assert.That(content.ContentType.Alias, Does.StartWith("Module").Or.StartWith(_projectInitials),
            string.Format("Document type alias '{0}' does not start with 'Module' or  '{1}' for document type with ID {2}", content.ContentType.Alias, _projectInitials, content.ContentType.Id));

        return ValueTask.CompletedTask;
    }
}
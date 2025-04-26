using NUnit.Framework;

internal sealed class PropertyAliasIsUniqueCheck : ContentCheckBase<DocumentTypeDetails> {
    private readonly HashSet<string> _seenAliases = new();
    private string? _documentTypeName = string.Empty;

    public PropertyAliasIsUniqueCheck(IReadOnlyCollection<IContentRequirementBase<DocumentTypeDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(DocumentTypeDetails content) {
        if (content.Property is null) {
            Assert.Fail($"Property is null for content in Document Type with name: {content.DocumentTypeName}");
            return ValueTask.CompletedTask;
        }
        // Check if the document type name has changed, if so, clear the seen aliases to start fresh for the new document type
        if (_documentTypeName != content.DocumentTypeName) {
            _seenAliases.Clear();
            _documentTypeName = content.DocumentTypeName;
        }

        Assert.That(_seenAliases.Add(content.Property.Alias), Is.True,
            $"Duplicate alias '{content.Property.Alias}' found in document type '{content.DocumentTypeName}'.");
        return ValueTask.CompletedTask;
    }
}
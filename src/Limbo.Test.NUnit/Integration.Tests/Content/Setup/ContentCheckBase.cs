// Abstract base class for performing content checks with requirements
internal abstract class ContentCheckBase<T>(IReadOnlyCollection<IContentRequirementBase<T>> _requirements) {

    // Validates the content by ensuring all requirements are met
    public ValueTask ValidateContentAsync(T content) {
        // Check each requirement
        foreach (var requirement in _requirements) {
            if (!requirement.IsRequirementMet(content)) {
                // If any requirement is not met, skip further validation
                return ValueTask.CompletedTask;
            }
        }
        // Perform the actual content validation
        return DoValidateContentAsync(content);
    }

    // Abstract method to be implemented by derived classes for specific validation logic
    protected abstract ValueTask DoValidateContentAsync(T content);
}
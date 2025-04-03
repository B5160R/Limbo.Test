internal abstract class ContentCheckBase<T> {
    private readonly IReadOnlyCollection<IContentRequirementBase<T>> _requirements;

    protected ContentCheckBase(IReadOnlyCollection<IContentRequirementBase<T>> requirements) {
        _requirements = requirements;
    }

    public ValueTask ValidateContentAsync(T content) {
        // 👇 1) Verify requirements
        if (!_requirements.All(r => r.IsRequirementMet(content))) return ValueTask.CompletedTask;

        // 👇 2) Perform the check
        return DoValidateContentAsync(content);
    }

    protected abstract ValueTask DoValidateContentAsync(T content);
}
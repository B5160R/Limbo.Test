// Interface defining a requirement for content validation
internal interface IContentRequirementBase<T> {
    // Checks if the requirement is met for the given content
    bool IsRequirementMet(T content);

    // Provides a description of the requirement (default implementation)
    string GetDescription() => GetType().Name;
}
internal interface IContentRequirementBase<T> {
    bool IsRequirementMet(T content);
}
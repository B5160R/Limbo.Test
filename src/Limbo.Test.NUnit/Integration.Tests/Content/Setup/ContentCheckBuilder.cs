using Microsoft.Extensions.DependencyInjection;

// Static factory for creating content check builders
internal static class ContentCheckBuilder {
    // Creates a new content check builder for the specified check and content types
    public static IContentCheckBuilder<TCheck, TContent> Create<TCheck, TContent>(IServiceProvider serviceProvider)
        where TCheck : ContentCheckBase<TContent>
        => new ContentCheckBuilder<TCheck, TContent>(serviceProvider);
}

// Interface defining a builder for content checks
internal interface IContentCheckBuilder<TCheck, TContent>
    where TCheck : ContentCheckBase<TContent> {

    // Adds a requirement to the builder by creating an instance of the requirement type
    IContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent>;

    // Adds an existing requirement instance to the builder
    IContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement);

    // Builds the content check with the specified requirements
    ContentCheckBase<TContent> Build();
}

// Implementation of the content check builder
internal sealed class ContentCheckBuilder<TCheck, TContent>(IServiceProvider _serviceProvider)
    : IContentCheckBuilder<TCheck, TContent>
    where TCheck : ContentCheckBase<TContent> {

    // List of requirements to be applied to the content check
    private readonly List<IContentRequirementBase<TContent>> _requirements = new();

    // Adds a requirement by creating an instance using the service provider
    public IContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent> {
        return WithRequirement(ActivatorUtilities.CreateInstance<TRequirement>(_serviceProvider, parameters));
    }

    // Adds an existing requirement instance to the list
    public IContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement) {
        _requirements.Add(requirement);
        return this;
    }

    // Builds the content check instance with the specified requirements
    public ContentCheckBase<TContent> Build() {
        return ActivatorUtilities.CreateInstance<TCheck>(_serviceProvider, _requirements);
    }
}
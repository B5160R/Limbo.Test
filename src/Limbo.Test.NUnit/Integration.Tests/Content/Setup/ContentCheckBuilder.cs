using Microsoft.Extensions.DependencyInjection;

internal static class ContentCheckBuilder {
    // 👇 A static method provides a convenient starting point for building content checks.
    // We simply instantiate a new builder and return it by its interface, so we can build against the interface.
    public static IContentCheckBuilder<TCheck, TContent> Create<TCheck, TContent>(IServiceProvider serviceProvider)
        where TCheck : ContentCheckBase<TContent>
        => new ContentCheckBuilder<TCheck, TContent>(serviceProvider);
}

// 👇 The interface defines how we build our content check
internal interface IContentCheckBuilder<TCheck, TContent>
    where TCheck : ContentCheckBase<TContent> {
    // 👇 Option 1: automatically instantiate a check with the given parameters
    IContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent>;

    // 👇 Option 2: create the requirement manually and pass it by hand
    IContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement);

    ContentCheckBase<TContent> Go();
}

internal sealed class ContentCheckBuilder<TCheck, TContent>
    : IContentCheckBuilder<TCheck, TContent>
    where TCheck : ContentCheckBase<TContent> {
    private readonly List<IContentRequirementBase<TContent>> _requirements;
    private readonly IServiceProvider _serviceProvider;

    public ContentCheckBuilder(IServiceProvider serviceProvider) {
        _requirements = new List<IContentRequirementBase<TContent>>();
        _serviceProvider = serviceProvider;
    }

    public IContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent> {
        // 👇 'ActivatorUtilities' is a utility from microsoft that makes it easier to instantiate objects of a given type, using a service provider to provide constructor parameters.
        // This is the part that allows us to use dependency injection inside a requirement
        return WithRequirement(ActivatorUtilities.CreateInstance<TRequirement>(_serviceProvider, parameters));
    }

    public IContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement) {
        _requirements.Add(requirement);
        return this;
    }

    public ContentCheckBase<TContent> Go() {
        // 👇 Once again, use the activator utilities to create an instance of the check.
        // This allows us to use dependency injection inside a rule
        return ActivatorUtilities.CreateInstance<TCheck>(_serviceProvider, _requirements);
    }
}
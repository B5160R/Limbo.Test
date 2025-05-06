using Microsoft.Extensions.DependencyInjection;

// Static factory for creating content check builders
internal static class ContentCheckBuilder {
    // Creates a new content check builder for the specified check and content types
    public static IPropertyContentCheckBuilder<TCheck, TContent> CreatePropertyCheck<TCheck, TContent>(IServiceProvider serviceProvider)
        where TCheck : PropertyContentCheck<TContent>
        => new PropertyContentCheckBuilder<TCheck, TContent>(serviceProvider);

    public static ISpaContentCheckBuilder<TCheck, TContent> CreateSpaCheck<TCheck, TContent>(IServiceProvider serviceProvider)
        where TCheck : SpaContentCheck<TContent>
        => new SpaContentCheckBuilder<TCheck, TContent>(serviceProvider);
}

// Interface defining a builder for content checks
internal interface IPropertyContentCheckBuilder<TCheck, TContent>
    where TCheck : PropertyContentCheck<TContent> {

    // Adds a requirement to the builder by creating an instance of the requirement type
    IPropertyContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent>;

    // Adds an existing requirement instance to the builder
    IPropertyContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement);

    IPropertyContentCheckBuilder<TCheck, TContent> WithProjectInitials(string projectInitials);

    // Builds the content check with the specified requirements
    PropertyContentCheck<TContent> Build();
}

internal interface ISpaContentCheckBuilder<TCheck, TContent>
    where TCheck : SpaContentCheck<TContent> {

    // Adds a requirement to the builder by creating an instance of the requirement type
    ISpaContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent>;

    // Adds an existing requirement instance to the builder
    ISpaContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement);

    // Builds the content check with the specified requirements
    SpaContentCheck<TContent> Build();
}

// Implementation of the content check builder
internal sealed class PropertyContentCheckBuilder<TCheck, TContent>(IServiceProvider _serviceProvider)
    : IPropertyContentCheckBuilder<TCheck, TContent>
    where TCheck : PropertyContentCheck<TContent> {

    // List of requirements to be applied to the content check
    private readonly List<IContentRequirementBase<TContent>> _requirements = new();
    private string _projectInitials = "";

    // Adds a requirement by creating an instance using the service provider
    public IPropertyContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent> {
        return WithRequirement(ActivatorUtilities.CreateInstance<TRequirement>(_serviceProvider, parameters));
    }

    // Adds an existing requirement instance to the list
    public IPropertyContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement) {
        _requirements.Add(requirement);
        return this;
    }

    public IPropertyContentCheckBuilder<TCheck, TContent> WithProjectInitials(string projectInitials) {
        _projectInitials = projectInitials;
        return this;
    }

    // Builds the content check instance with the specified requirements
    public PropertyContentCheck<TContent> Build() {
        return ActivatorUtilities.CreateInstance<TCheck>(_serviceProvider, _requirements, _projectInitials);
    }
}

internal sealed class SpaContentCheckBuilder<TCheck, TContent>(IServiceProvider _serviceProvider)
    : ISpaContentCheckBuilder<TCheck, TContent>
    where TCheck : SpaContentCheck<TContent> {

    // List of requirements to be applied to the content check
    private readonly List<IContentRequirementBase<TContent>> _requirements = new();

    // Adds a requirement by creating an instance using the service provider
    public ISpaContentCheckBuilder<TCheck, TContent> WithRequirement<TRequirement>(params object[] parameters)
        where TRequirement : IContentRequirementBase<TContent> {
        return WithRequirement(ActivatorUtilities.CreateInstance<TRequirement>(_serviceProvider, parameters));
    }

    // Adds an existing requirement instance to the list
    public ISpaContentCheckBuilder<TCheck, TContent> WithRequirement(IContentRequirementBase<TContent> requirement) {
        _requirements.Add(requirement);
        return this;
    }

    // Builds the content check instance with the specified requirements
    public SpaContentCheck<TContent> Build() {
        return ActivatorUtilities.CreateInstance<TCheck>(_serviceProvider, _requirements);
    }
}
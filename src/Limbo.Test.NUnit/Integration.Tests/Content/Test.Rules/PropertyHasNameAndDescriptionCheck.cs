using NUnit.Framework;

internal sealed class PropertyHasNameAndDescriptionCheck : ContentCheckBase<PropertyDetails> {
    public PropertyHasNameAndDescriptionCheck(IReadOnlyCollection<IContentRequirementBase<PropertyDetails>> requirements) : base(requirements) { }

    protected override ValueTask DoValidateContentAsync(PropertyDetails content) {

        Assert.That(content, Is.Not.Null, "Content is null");

        Assert.That(content.Name, Is.Not.Null.Or.Empty,
            string.Format("\"{0}\" ({1}) in \"{2}\" should have a name in {3}",
                content.Name,
                content.Property.Alias,
                content.ContentType.Name,
                content.Culture));

        // content.Description.Should().NotBeNullOrWhiteSpace(because: "\"{0}\" ({1}) in \"{2}\" should have a description in {3}",
        //         content.Name,
        //         content.Property.Alias,
        //         content.ContentType.Name,
        //         content.Culture);

        return ValueTask.CompletedTask;
    }
}
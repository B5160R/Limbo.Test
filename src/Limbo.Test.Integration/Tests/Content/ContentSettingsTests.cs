using Herningsholm.Web;
using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

// Integration tests for validating Umbraco content settings
[TestFixture]
[Parallelizable(ParallelScope.All)]
[Category("Integration")]
[Description("Integration tests for validating Umbraco content settings.")]
[TestOf(typeof(ContentSettingsTests))]
public class ContentSettingsTests : IntegrationTestBase<Program> {
    private IContentTypeService _contentTypeService;
    private IEnumerable<IContentType> _contentTypes;

    [SetUp]
    public override void Setup() {
        base.Setup();
        _contentTypeService = GetService<IContentTypeService>();
        _contentTypes = _contentTypeService.GetAll();
    }

    [Test]
    [TestCase(TestName = "Validate all Umbraco document type elements has Alias beginning with 'Block'")]
    public async Task Validate_all_Umbraco_document_type_elements_has_Alias_beginning_with_Block() {
        // Arrange requiredments for document type checks
        var isElementType = new IsElementType();
        var doesNotUseAliasSeo = new UsesAlias("seo", _invert: true);
        var doesNotUseAliasSettings = new UsesAlias("settings", _invert: true);

        // Build content check
        PropertyContentCheck<DocumentTypeDetails> contentCheck = ContentCheckBuilder
            .CreatePropertyCheck<DocumentTypeAliasBeginsWithBlockCheck, DocumentTypeDetails>(ServiceProvider)
            .WithProjectInitials("he")
            .WithRequirement(isElementType)
            .WithRequirement(doesNotUseAliasSeo)
            .WithRequirement(doesNotUseAliasSettings)
            .Build();

        // Act: Validate each document type and its properties
        await contentCheck.RunAssertionsAsync(_contentTypes, false);
    }

    [Test]
    [TestCase(TestName = "Validate all Umbraco document type none elements has Alias beginning with either project initials or 'Module'")]
    public async Task Validate_all_Umbraco_document_type_none_elements_has_Alias_beginning_with_either_project_initials_or_Module() {
        // Arrange requiredments for document type checks
        var isNotElementType = new IsElementType(_invert: true);

        // Build content check
        PropertyContentCheck<DocumentTypeDetails> contentCheck = ContentCheckBuilder
            .CreatePropertyCheck<DocumentTypeAliasBeginsWithProjectInitialsOrModuleCheck, DocumentTypeDetails>(ServiceProvider)
            .WithProjectInitials("he")
            .WithRequirement(isNotElementType)
            .Build();

        // Act: Validate each document type and its properties
        await contentCheck.RunAssertionsAsync(_contentTypes, false);
    }

    [Test]
    [TestCase(TestName = "Validate all Umbraco document type properties has Name and Alias")]
    public async Task Validate_all_Umbraco_document_type_properties_has_Name_and_Alias() {
        // Arrange
        // Build content check
        PropertyContentCheck<DocumentTypeDetails> contentCheck = ContentCheckBuilder
            .CreatePropertyCheck<PropertyHasNameAndAliasCheck, DocumentTypeDetails>(ServiceProvider)
            .Build();

        // Act: Validate each document type and its properties
        await contentCheck.RunAssertionsAsync(_contentTypes, true);
    }

    [Test]
    [TestCase(TestName = "Validate all Umbraco document type properties has unique Alias")]
    public async Task Validate_all_Umbraco_document_type_properties_has_unique_Alias() {
        // Arrange
        // Build content check
        PropertyContentCheck<DocumentTypeDetails> contentCheck = ContentCheckBuilder
            .CreatePropertyCheck<PropertyAliasIsUniqueCheck, DocumentTypeDetails>(ServiceProvider)
            .Build();

        // Act: Validate each document type and its properties
        await contentCheck.RunAssertionsAsync(_contentTypes, true);
    }

    [Test]
    [TestCase(TestName = "Validate all Umbraco document type properties uses Limbo property editor where required")]
    public async Task Validate_all_Umbraco_document_type_properties_uses_Limbo_Property_Editor_Where_Required() {
        // Arrange requirements for property checks
        var doesNotUseMultipleTextstring = new UsesPropertyEditor("Umbraco.MultipleTextstring", _invert: true);
        var doesNotUseRadioButtonList = new UsesPropertyEditor("Umbraco.RadioButtonList", _invert: true);
        var doesNotUseUserPicker = new UsesPropertyEditor("Umbraco.UserPicker", _invert: true);
        var doesNotUseLabel = new UsesPropertyEditor("Umbraco.Label", _invert: true);
        var doesNotUseContactPersonPicker = new UsesPropertyEditor("ContactPersonPicker", _invert: true);
        var doesNotUseTinyMce = new UsesPropertyEditor("Umbraco.TinyMCE", _invert: true);
        var doesNotUseDateTime = new UsesPropertyEditor("Umbraco.DateTime", _invert: true);
        var doesNotUseColorGuide = new UsesPropertyEditor("custom.ColorGuide", _invert: true);
        var doesNotUseFormPicker = new UsesPropertyEditor("UmbracoForms.FormPicker", _invert: true);
        var doesNotUseDataList = new UsesPropertyEditor("Umbraco.Community.Contentment.DataList", _invert: true);
        var doesNotUseBlocklist = new UsesPropertyEditor("Umbraco.BlockList", _invert: true);

        // Build content check
        PropertyContentCheck<DocumentTypeDetails> contentCheck = ContentCheckBuilder
            .CreatePropertyCheck<PropertyUsesLimboPropertyEditorCheck, DocumentTypeDetails>(ServiceProvider)
            .WithRequirement(doesNotUseMultipleTextstring)
            .WithRequirement(doesNotUseRadioButtonList)
            .WithRequirement(doesNotUseUserPicker)
            .WithRequirement(doesNotUseLabel)
            .WithRequirement(doesNotUseContactPersonPicker)
            .WithRequirement(doesNotUseTinyMce)
            .WithRequirement(doesNotUseDateTime)
            .WithRequirement(doesNotUseBlocklist)
            .WithRequirement(doesNotUseColorGuide)
            .WithRequirement(doesNotUseFormPicker)
            .WithRequirement(doesNotUseDataList)
            .Build();

        await contentCheck.RunAssertionsAsync(_contentTypes, true);
    }
}
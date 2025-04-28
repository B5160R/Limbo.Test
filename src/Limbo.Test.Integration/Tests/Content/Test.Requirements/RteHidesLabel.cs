using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

// Requirement to check if the "Hide Label" setting is correctly configured for TinyMCE properties
internal sealed class RteHidesLabel : IContentRequirementBase<DocumentTypeDetails> {
    private readonly IDataTypeService _dataTypeService;
    private readonly bool _invert;

    // Constructor to initialize the data type service and invert flag
    public RteHidesLabel(IDataTypeService dataTypeService, bool invert = false) {
        _dataTypeService = dataTypeService;
        _invert = invert;
    }

    // Checks if the "Hide Label" setting matches the expected value
    public bool IsRequirementMet(DocumentTypeDetails content) {
        // Skip validation if the property is not TinyMCE
        if (content.Property?.PropertyEditorAlias is not "Umbraco.TinyMCE") return true;

        // Get the data type configuration
        var dataType = _dataTypeService.GetDataType(content.Property.DataTypeId);
        if (dataType is null) return true;

        // Get the TinyMCE configuration
        var rteConfig = dataType.ConfigurationAs<RichTextConfiguration>();
        if (rteConfig is null) return true;

        // Check if the "Hide Label" setting matches the expected value
        bool shouldBeHidden = !_invert;
        return rteConfig.HideLabel == shouldBeHidden;
    }
}
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

internal sealed class RteHidesLabel : IContentRequirementBase<PropertyDetails> {
    private readonly IDataTypeService _dataTypeService;
    private readonly bool _invert;

    public RteHidesLabel(IDataTypeService dataTypeService, bool invert = false) {
        _dataTypeService = dataTypeService;
        _invert = invert;
    }

    public bool IsRequirementMet(PropertyDetails content) {
        // 👇 This requirement only applies to rich text editor properties, so we need to make sure that the data type is a rich text editor
        if (content.Property.PropertyEditorAlias is not "Umbraco.TinyMCE") return true;

        // 👇 Use the datatype service to find the rich text editor settings
        var dataType = _dataTypeService.GetDataType(content.Property.DataTypeId);
        if (dataType is null) return true;

        // 👇 Finally read the config from the rich text editor to find out whether or not the label has been hidden
        var rteConfig = dataType.ConfigurationAs<RichTextConfiguration>();
        if (rteConfig is null) return true;

        bool shouldBeHidden = !_invert;

        return rteConfig.HideLabel == shouldBeHidden;
    }
}
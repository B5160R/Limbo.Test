using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class Welcome {
    [JsonProperty("meta")]
    public WelcomeMeta Meta { get; set; }

    [JsonProperty("pageId")]
    public long PageId { get; set; }

    [JsonProperty("pageKey")]
    public Guid PageKey { get; set; }

    [JsonProperty("siteId")]
    public long SiteId { get; set; }

    [JsonProperty("siteKey")]
    public Guid SiteKey { get; set; }

    [JsonProperty("contentGuid")]
    public Guid ContentGuid { get; set; }

    [JsonProperty("executeTimeMs")]
    public long ExecuteTimeMs { get; set; }

    [JsonProperty("site")]
    public Site Site { get; set; }

    [JsonProperty("content")]
    public WelcomeContent Content { get; set; }
}

public partial class WelcomeContent {
    [JsonProperty("meta")]
    public ContentMeta Meta { get; set; }

    [JsonProperty("synonyms")]
    public object[] Synonyms { get; set; }

    [JsonProperty("ogImage")]
    public object OgImage { get; set; }

    [JsonProperty("umbracoNaviHide")]
    public bool UmbracoNaviHide { get; set; }

    [JsonProperty("hideFromSearch")]
    public bool HideFromSearch { get; set; }

    [JsonProperty("hideFromSitemap")]
    public bool HideFromSitemap { get; set; }

    [JsonProperty("hideWords")]
    public object[] HideWords { get; set; }

    [JsonProperty("interval")]
    public object Interval { get; set; }

    [JsonProperty("editor")]
    public long Editor { get; set; }

    [JsonProperty("selectedTheme")]
    public object SelectedTheme { get; set; }

    [JsonProperty("hero")]
    public Hero Hero { get; set; }

    [JsonProperty("showLongRead")]
    public bool ShowLongRead { get; set; }

    [JsonProperty("contentElements")]
    public FluffyContentElement[] ContentElements { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("breadcrumb")]
    public PageNotFoundPage[] Breadcrumb { get; set; }

    [JsonProperty("path")]
    public PageNotFoundPage[] Path { get; set; }

    [JsonProperty("pageTitle")]
    public string PageTitle { get; set; }

    [JsonProperty("wordList")]
    public string[] WordList { get; set; }

    [JsonProperty("showStickers")]
    public bool ShowStickers { get; set; }

    [JsonProperty("theme")]
    public string Theme { get; set; }

    [JsonProperty("subEducations")]
    public object[] SubEducations { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("level")]
    public long Level { get; set; }

    [JsonProperty("created")]
    public DateTimeOffset Created { get; set; }

    [JsonProperty("updated")]
    public DateTimeOffset Updated { get; set; }

    [JsonProperty("template")]
    public string Template { get; set; }
}

public partial class PageNotFoundPage {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("udiKey")]
    public string UdiKey { get; set; }

    [JsonProperty("hasExternalOutboundRedirect")]
    public bool HasExternalOutboundRedirect { get; set; }

    [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
    public PageNotFoundPageContent Content { get; set; }
}

public partial class PageNotFoundPageContent {
    [JsonProperty("meta")]
    public object Meta { get; set; }

    [JsonProperty("umbracoNaviHide")]
    public bool UmbracoNaviHide { get; set; }

    [JsonProperty("hideFromSearch")]
    public bool HideFromSearch { get; set; }

    [JsonProperty("hideFromSitemap")]
    public bool HideFromSitemap { get; set; }

    [JsonProperty("hideWords")]
    public object[] HideWords { get; set; }

    [JsonProperty("interval")]
    public object Interval { get; set; }

    [JsonProperty("editor")]
    public long Editor { get; set; }

    [JsonProperty("synonyms")]
    public object[] Synonyms { get; set; }

    [JsonProperty("ogImage")]
    public object OgImage { get; set; }

    [JsonProperty("hero")]
    public Hero Hero { get; set; }

    [JsonProperty("showLongRead")]
    public bool ShowLongRead { get; set; }

    [JsonProperty("contentElements")]
    public PurpleContentElement[] ContentElements { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("breadcrumb")]
    public object Breadcrumb { get; set; }

    [JsonProperty("path")]
    public object Path { get; set; }

    [JsonProperty("pageTitle")]
    public string PageTitle { get; set; }

    [JsonProperty("wordList")]
    public object[] WordList { get; set; }

    [JsonProperty("showStickers")]
    public bool ShowStickers { get; set; }

    [JsonProperty("theme")]
    public string Theme { get; set; }

    [JsonProperty("subEducations")]
    public object SubEducations { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("level")]
    public long Level { get; set; }

    [JsonProperty("created")]
    public DateTimeOffset Created { get; set; }

    [JsonProperty("updated")]
    public DateTimeOffset Updated { get; set; }

    [JsonProperty("template")]
    public string Template { get; set; }
}

public partial class PurpleContentElement {
    [JsonProperty("alias")]
    public string Alias { get; set; }

    [JsonProperty("content")]
    public PurpleContent Content { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }
}

public partial class PurpleContent {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}

public partial class Hero {
    [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
    public string Alias { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("teaser")]
    public string Teaser { get; set; }

    [JsonProperty("media")]
    public object Media { get; set; }

    [JsonProperty("showBreadcrumb")]
    public bool ShowBreadcrumb { get; set; }

    [JsonProperty("showFullWidth")]
    public bool ShowFullWidth { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }

    [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
    public Hero Content { get; set; }
}

public partial class FluffyContentElement {
    [JsonProperty("alias")]
    public string Alias { get; set; }

    [JsonProperty("content")]
    public FluffyContent Content { get; set; }

    [JsonProperty("key")]
    public Guid Key { get; set; }
}

public partial class FluffyContent {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("definitionsUrl")]
    public string DefinitionsUrl { get; set; }

    [JsonProperty("entriesUrl")]
    public string EntriesUrl { get; set; }
}

public partial class ContentMeta {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("htmlAttrs")]
    public HtmlAttrs HtmlAttrs { get; set; }

    [JsonProperty("link")]
    public MetaLink[] Link { get; set; }

    [JsonProperty("meta")]
    public MetaElement[] Meta { get; set; }
}

public partial class HtmlAttrs {
    [JsonProperty("lang")]
    public Lang Lang { get; set; }
}

public partial class MetaLink {
    [JsonProperty("hid")]
    public string Hid { get; set; }

    [JsonProperty("rel")]
    public string Rel { get; set; }

    [JsonProperty("href")]
    public Uri Href { get; set; }
}

public partial class MetaElement {
    [JsonProperty("hid")]
    public string Hid { get; set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("property", NullValueHandling = NullValueHandling.Ignore)]
    public string Property { get; set; }
}

public partial class WelcomeMeta {
    [JsonProperty("code")]
    public long Code { get; set; }
}

public partial class Site {
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("pageNotFoundPage")]
    public PageNotFoundPage PageNotFoundPage { get; set; }

    [JsonProperty("header")]
    public Header Header { get; set; }

    [JsonProperty("navigation")]
    public Navigation Navigation { get; set; }

    [JsonProperty("footer")]
    public Footer Footer { get; set; }
}

public partial class Footer {
    [JsonProperty("globalFooter")]
    public GlobalFooter GlobalFooter { get; set; }

    [JsonProperty("subFooter")]
    public object SubFooter { get; set; }

    [JsonProperty("color")]
    public object Color { get; set; }
}

public partial class GlobalFooter {
    [JsonProperty("linkListColumn1")]
    public LinkListColumn LinkListColumn1 { get; set; }

    [JsonProperty("linkListColumn2")]
    public LinkListColumn LinkListColumn2 { get; set; }

    [JsonProperty("linkListColumn3")]
    public LinkListColumn LinkListColumn3 { get; set; }

    [JsonProperty("addressColumn1")]
    public AddressColumn1 AddressColumn1 { get; set; }

    [JsonProperty("addressColumn2")]
    public AddressColumn2 AddressColumn2 { get; set; }
}

public partial class AddressColumn1 {
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }
}

public partial class AddressColumn2 {
    [JsonProperty("email")]
    public string[] Email { get; set; }
}

public partial class LinkListColumn {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("links")]
    public LogoLinkElement[] Links { get; set; }
}

public partial class LogoLinkElement {
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}

public partial class Header {
    [JsonProperty("logo")]
    public object Logo { get; set; }

    [JsonProperty("logoLink")]
    public LogoLinkElement LogoLink { get; set; }

    [JsonProperty("quickMenu")]
    public QuickMenu QuickMenu { get; set; }

    [JsonProperty("searchPage")]
    public QuickMenu SearchPage { get; set; }

    [JsonProperty("showQuickMenu")]
    public bool ShowQuickMenu { get; set; }

    [JsonProperty("color")]
    public object Color { get; set; }
}

public partial class QuickMenu {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("children")]
    public QuickMenu[] Children { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("parentId")]
    public long ParentId { get; set; }

    [JsonProperty("template")]
    public string Template { get; set; }

    [JsonProperty("culture")]
    public Lang Culture { get; set; }

    [JsonProperty("hasChildren")]
    public bool HasChildren { get; set; }

    [JsonProperty("isVisible")]
    public bool IsVisible { get; set; }
}

public partial class Navigation {
    [JsonProperty("primary")]
    public QuickMenu[] Primary { get; set; }

    [JsonProperty("secondary")]
    public QuickMenu[] Secondary { get; set; }

    [JsonProperty("mainSiteLink")]
    public object MainSiteLink { get; set; }
}

public enum Lang { Da };

internal static class Converter {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
                LangConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class LangConverter : JsonConverter {
    public override bool CanConvert(Type t) => t == typeof(Lang) || t == typeof(Lang?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        if (value == "da") {
            return Lang.Da;
        }
        throw new Exception("Cannot unmarshal type Lang");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer) {
        if (untypedValue == null) {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Lang) untypedValue;
        if (value == Lang.Da) {
            serializer.Serialize(writer, "da");
            return;
        }
        throw new Exception("Cannot marshal type Lang");
    }

    public static readonly LangConverter Singleton = new LangConverter();
}
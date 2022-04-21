using System.Xml.Linq;

namespace EasyI18n;

internal class EasyI18NFormatReader
{
    internal string? GetAttribute(
        XElement element,
        string attributeName)
    {
        var found = element.Attributes()
            .FirstOrDefault(_ => _.Name.LocalName.Equals(attributeName));
        return found?.Value;
    }

    internal bool GetAttributeBool(
        XElement element,
        string attributeName)
    {
        var value = GetAttribute(element, attributeName) ?? "false";
        return value.ToLowerInvariant() switch
        {
            "true" => true,
            "yes" => true,
            _ => false,
        };
    }

    internal KeyMessage[] Read(string xmlContent)
    {
        return Read(XDocument.Parse(xmlContent));
    }

    internal KeyMessage[] Read(XDocument document)
    {
        var messageNodes = document.Root!
            .Elements()
            .Where(_ => _.Name.LocalName.Equals("message"))
            .ToArray();

        var result = new List<KeyMessage>();
        foreach (var node in messageNodes)
        {
            var key = GetAttribute(node, "key");
            if (key == null)
            {
                continue;
            }

            var locales = node.Elements()
                .Where(_ => _.Name.LocalName.Equals("locale"))
                .Select(_ => new LocaleMessage
                {
                    Locale = GetAttribute(_, "lang") ?? "",
                    Message = _.Value
                })
                .Where(_ => !string.IsNullOrWhiteSpace(_.Locale))
                .ToArray();

            result.Add(new KeyMessage
            {
                Key = key,
                Messages = locales
            });
        }

        return result.ToArray();
    }
}
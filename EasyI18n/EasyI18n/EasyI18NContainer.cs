using System.Globalization;

namespace EasyI18n;

public class EasyI18NContainer : IEasyI18N
{
    readonly string _defaultLocale;
    readonly Dictionary<string, Dictionary<string, LocaleMessage>> _messages = new();
    string _locale;

    public EasyI18NContainer(
        string defaultLocale = "de")
    {
        _locale = defaultLocale?.ToLowerInvariant() ?? "de";
        _defaultLocale = _locale;
        FormatProvider = new CultureInfo(_defaultLocale);
    }

    public event EventHandler? LocaleChanged;

    public string DefaultLocale => _defaultLocale;
    public IFormatProvider FormatProvider { get; private set; }
    public string Locale => _locale;

    public void AddMessages(KeyMessage[] messages)
    {
        // Check for duplicate keys first
        var existingKeys = _messages.Keys.Select(_ => _).ToArray();
        var newKeys = messages.Select(_ => _.Key).ToArray();

        var duplicateKeys = existingKeys.Intersect(newKeys).ToArray();
        if (duplicateKeys.Any())
        {
            throw new DuplicateKeyException(duplicateKeys);
        }

        foreach (var _ in messages)
        {
            _messages.Add(_.Key, _.Messages.ToDictionary(_ => _.Locale));
        }
    }

    public string GetText(string key)
    {
        if (_messages.TryGetValue(key, out var messages))
        {
            if (messages.TryGetValue(_locale, out var found)
                || messages.TryGetValue(_defaultLocale, out found))
            {
                return found.Message;
            }
        }

        return $"<{key} NOT FOUND>";
    }

    public void SetLocale(string locale)
    {
        if (string.IsNullOrWhiteSpace(locale)
            || locale.Equals(_locale, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        _locale = locale.ToLowerInvariant();
        FormatProvider = new CultureInfo(_locale);
        LocaleChanged?.Invoke(this, EventArgs.Empty);
    }
}
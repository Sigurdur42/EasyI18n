namespace EasyI18n;

public class LocaleMessage
{
    public LocaleMessage()
    {
    }

    public LocaleMessage(string locale, string message)
    {
        Locale = locale;
        Message = message;
    }

    public string Locale { get; set; } = "";
    public string Message { get; set; } = "";

    public string GenerateCode()
    {
        return $"new LocaleMessage(\"{Locale}\", \"{Message}\")";
    }
}

public class KeyMessage
{
    public KeyMessage()
    {
    }

    public KeyMessage(string key, LocaleMessage[] messages)
    {
        Key = key;
        Messages = messages;
    }

    public string Key { get; set; } = "";
    public LocaleMessage[] Messages { get; set; } = Array.Empty<LocaleMessage>();

    public string GenerateCode()
    {
        return $"new KeyMessage(\"{Key}\", new[] {{ {string.Join(", ", Messages.Select(_ => _.GenerateCode()))} }})";
    }
}
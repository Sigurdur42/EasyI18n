namespace EasyI18n;

public interface IEasyI18N
{
    event EventHandler? LocaleChanged;

    string DefaultLocale { get; }
    IFormatProvider FormatProvider { get; }
    string Locale { get; }

    void AddMessages(KeyMessage[] messages);

    string GetText(string key);

    void SetLocale(string locale);
}
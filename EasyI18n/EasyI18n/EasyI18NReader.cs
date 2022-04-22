namespace EasyI18n;

public interface IEasyI18NReader
{
    void ReadFromFile(FileInfo languageFile);
}

public class EasyI18NReader : IEasyI18NReader
{
    readonly IEasyI18N? _easyI18N;

    public EasyI18NReader(IEasyI18N? easyI18N)
    {
        _easyI18N = easyI18N;
    }

    /// <summary>
    /// Reads the language file and adds the found messages to the global EasyI18N instance.
    /// </summary>
    public void ReadFromFile(FileInfo languageFile)
    {
        if (!languageFile.Exists)
        {
            throw new FileNotFoundException($"Cannot find language file '{languageFile}'", languageFile.FullName);
        }

        var content = File.ReadAllText(languageFile.FullName);

        var reader = new EasyI18NFormatReader();

        var messages = reader.Read(content);
        _easyI18N?.AddMessages(messages);
    }
}
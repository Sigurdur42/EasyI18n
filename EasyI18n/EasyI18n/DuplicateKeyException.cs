namespace EasyI18n;

public class DuplicateKeyException : Exception
{
    public string[] DuplicateKeys { get; }

    public DuplicateKeyException(
        IEnumerable<string> duplicateKeys)
    : base($"EasyI18N: You are trying to add duplicate keys ({string.Join(", ", duplicateKeys)}). {Environment.NewLine}Keys have to be unique - please ensure that all files contain only unique keys.")
    {
        DuplicateKeys = duplicateKeys.ToArray();
    }
}
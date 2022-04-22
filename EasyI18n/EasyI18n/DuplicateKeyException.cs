namespace EasyI18n;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Roslynator", 
    "RCS1194:Implement exception constructors.", 
    Justification = "The default constructors do not make sense as they miss the important information (the duplicate keys)")]
public class DuplicateKeyException : Exception
{
    public DuplicateKeyException(
        IEnumerable<string> duplicateKeys)
    : base($"EasyI18N: You are trying to add duplicate keys ({string.Join(", ", duplicateKeys)}). {Environment.NewLine}Keys have to be unique - please ensure that all files contain only unique keys.")
    {
        DuplicateKeys = duplicateKeys.ToArray();
    }

    public string[] DuplicateKeys { get; }
}
using EasyI18n;

namespace EasyI18N.Generator;

public class SingleCodeFile
{
    public string? ClassName { get; set; }
    public string? CodeContent { get; set; }
    public string? ErrorDetails { get; set; }
    public string? FileName { get; set; }
    public bool Success { get; set; }
}

public class GeneratedCode
{
    public string? ErrorDetails { get; set; }
    public SingleCodeFile ExtensionClass { get; set; } = new SingleCodeFile();
    public List<KeyMessage> Messages { get; } = new List<KeyMessage>();
    public bool Success { get; set; }
    public SingleCodeFile ViewModel { get; set; } = new SingleCodeFile();
}
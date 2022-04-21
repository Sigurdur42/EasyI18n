using System.Text;
using System.Xml.Linq;
using EasyI18n;

namespace EasyI18N.Generator;

internal class GenerateCodeForXml
{
    internal static string MakeSafe(string key)
    {
        return key
                .Replace(" ", "")
                .Replace("\t", "")
                .Replace("-", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":", "")
            ;
    }

    internal GeneratedCode GenerateCode(FileInfo inputFile)
    {
        var result = new GeneratedCode();
        try
        {
            var reader = new EasyI18NFormatReader();
            var document = XDocument.Load(inputFile.FullName);
            var generateViewModel = reader.GetAttributeBool(document.Root, "generateViewModel");

            var parts = reader.Read(document);

            result.Messages.AddRange(parts);

            result.ExtensionClass = GenerateExtensionMethod(parts, inputFile);
            if (generateViewModel)
            {
                result.ViewModel = GenerateViewModel(parts, inputFile);
            }
            else
            {
                result.ViewModel.Success = true;
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorDetails = $"error creating code for {inputFile}:" + ex;

            return result;
        }

        var allErrors = new[]
        {
            result.ExtensionClass.ErrorDetails,
            result.ViewModel.ErrorDetails,
        };

        result.Success = result.ExtensionClass.Success
            && result.ViewModel.Success;

        result.ErrorDetails = string.Join(Environment.NewLine, allErrors.Where(_ => !string.IsNullOrWhiteSpace(_)));
        return result;
    }

//     internal SingleCodeFile GenerateDiExtension(
//         GeneratedCode[] generated,
//         string projectName)
//     {
//         var result = new SingleCodeFile();
//         try
//         {
//             var diLines = generated
//                 .Select(_ => _.ViewModel.ClassName)
//                 .Where(_ => !string.IsNullOrWhiteSpace(_))
//                 .Select(_ => $"            _.AddSingleton<{_}>();")
//                 .ToArray();
//
//             var safeProjectName = MakeSafe(projectName);
//             result.FileName = safeProjectName + ".DiExtension.g.cs";
//
//             result.CodeContent = @"using Microsoft.Extensions.DependencyInjection;
//
// namespace EasyI18n
// {
//     public static partial class EasyI18NExtensions
//     {
//         public static void AddEasyI18N%%PROJECTNAME%%(this IServiceCollection _)
//         {
// %%DICONTENT%%
//         }
//     }
// }"
//                 .Replace("%%PROJECTNAME%%", safeProjectName)
//                 .Replace("%%DICONTENT%%", string.Join(Environment.NewLine, diLines))
//                 ;
//
//             result.Success = true;
//         }
//         catch (Exception ex)
//         {
//             result.Success = false;
//             result.ErrorDetails = $"error creating di extensions for {projectName}:" + ex;
//         }
//
//         return result;
//     }

    private static string EscapeMessageForCode(string message)
        => message.Replace("\"", "\"\"");

    private SingleCodeFile GenerateExtensionMethod(KeyMessage[] parts, FileInfo inputFile)
    {
        var result = new SingleCodeFile();
        try
        {
            var codeLines = parts
                .Select(_ =>
                {
                    var builder = new StringBuilder();
                    builder.AppendLine($"        public static string Get{MakeSafe(_.Key)}(this IEasyI18N _) => _.Locale switch");
                    builder.AppendLine("        {");
                    foreach (var locale in _.Messages)
                    {
                        builder.Append("            ");
                        builder.Append('"');
                        builder.Append(locale.Locale.ToLowerInvariant());
                        builder.Append('"');
                        builder.Append(" => @");
                        builder.Append('"');
                        builder.Append(EscapeMessageForCode(locale.Message));
                        builder.Append('"');
                        builder.Append(',');
                        builder.AppendLine();
                    }

                    var defaultMessage = _.Messages
                        .FirstOrDefault(_ => _.Locale.Equals("de", StringComparison.InvariantCultureIgnoreCase))
                        ?? _.Messages.FirstOrDefault();

                    builder.Append("            ");
                    builder.Append("_");
                    builder.Append(" => @");
                    builder.Append('"');
                    builder.Append(EscapeMessageForCode(defaultMessage?.Message ?? ""));
                    builder.Append('"');
                    builder.Append(',');
                    builder.AppendLine();

                    builder.Append("        ");
                    builder.AppendLine("};");

                    return builder.ToString();
                })
                .ToArray();

            var className = MakeSafe(Path.GetFileNameWithoutExtension(inputFile.FullName));
            result.FileName = className + ".g.cs";
            result.ClassName = className;
            result.CodeContent = @"namespace EasyI18n
{
    public static class %%CLASSNAME%%
    {
%%CLASSCONTENT%%
    }
}"
                .Replace("%%CLASSNAME%%", className)
                .Replace("%%CLASSCONTENT%%", string.Join(Environment.NewLine, codeLines));

            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorDetails = $"error creating code for {inputFile}:" + ex;
        }

        return result;
    }

    private SingleCodeFile GenerateViewModel(KeyMessage[] parts, FileInfo inputFile)
    {
        var result = new SingleCodeFile();
        try
        {
            var keys = parts
                .Select(_ => _.Key)
                .ToArray();

            var codeLines = keys
                .Select(_ => $"        public string {MakeSafe(_)} => _easyI18N.GetText(\"{_}\");")
                .ToArray();

            var localeChanged = keys
            .Select(_ => $"                OnPropertyChanged(\"{MakeSafe(_)}\");")
            .ToArray();

            var messageParts = GetMessageDeclarations(parts);

            var className = MakeSafe(Path.GetFileNameWithoutExtension(inputFile.FullName)) + "ViewModel";
            result.FileName = className + ".g.cs";
            result.ClassName = className;
            result.CodeContent = @"using Microsoft.Extensions.Logging;

namespace EasyI18n
{
    public class %%CLASSNAME%% : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private readonly IEasyI18N _easyI18N;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

        public %%CLASSNAME%%(
            IEasyI18N easyI18N,
            ILogger<%%CLASSNAME%%> logger)
        {
            _easyI18N = easyI18N;

            _easyI18N.LocaleChanged += (_, _) =>
            {
                logger?.LogInformation(""[EasyI18N] Locale changed registered in %%CLASSNAME%%"");

%%NOTIFYLOCALECHANGED%%
            };
        }

%%CLASSCONTENT%%
    }
}"
                .Replace("%%CLASSNAME%%", className)
                .Replace("%%MESSAGES%%", messageParts)
                .Replace("%%CLASSCONTENT%%", string.Join(Environment.NewLine, codeLines))
                .Replace("%%NOTIFYLOCALECHANGED%%", string.Join(Environment.NewLine, localeChanged))
                ;

            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorDetails = $"error creating code for {inputFile}:" + ex;
        }

        return result;
    }

    private string GetMessageDeclarations(KeyMessage[] parts)
         => $@"new[]{Environment.NewLine}{{{Environment.NewLine}    {string.Join("," + Environment.NewLine + "    ", parts.Select(_ => _.GenerateCode()))}{Environment.NewLine}}}";
}
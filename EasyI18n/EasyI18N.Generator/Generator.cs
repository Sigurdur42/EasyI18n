using System.Text;
using Microsoft.CodeAnalysis;

namespace EasyI18N.Generator;

[Generator]
public class Generator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
//#if DEBUG
//        if (!System.Diagnostics.Debugger.IsAttached)
//        {
//            System.Diagnostics.Debugger.Launch();
//        }
//#endif

        var builder = new StringBuilder();

        var xmlFiles = context.AdditionalFiles
            .Where(_ => Path.GetExtension(_.Path).Equals(".xml", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        builder.AppendLine($"Found xml files: {Environment.NewLine}{string.Join(",", xmlFiles.Select(_ => _.Path))}");

        var generated = new List<GeneratedCode>();
        var generator = new GenerateCodeForXml();
        foreach (var additionalText in xmlFiles)
        {
            var _ = generator.GenerateCode(new FileInfo(additionalText.Path));
            if (_.Success)
            {
                AddGenerated(builder, context, _.ExtensionClass);
                AddGenerated(builder, context, _.ViewModel);
            }
            else
            {
                builder.AppendLine($"error in code generation: {_.ErrorDetails}");
            }

            generated.Add(_);
        }

        // var di = generator.GenerateDiExtension(
        //     generated.ToArray(),
        //     context.Compilation.SourceModule.ContainingSymbol.Name);
        // if (di.Success)
        // {
        //     AddGenerated(builder, context, di);
        // }
        // else
        // {
        //     builder.AppendLine($"error in DI generation: {di.ErrorDetails}");
        // }

        context.AddSource($"Debug.g.cs", "/*" + builder + "*/");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }

    void AddGenerated(
        StringBuilder analysisBuilder,
        GeneratorExecutionContext context,
        SingleCodeFile codeFile)
    {
        if (codeFile.Success && !string.IsNullOrEmpty(codeFile.CodeContent))
        {
            analysisBuilder.AppendLine($"Adding code for {codeFile.FileName}...");
            context.AddSource(codeFile.FileName!, codeFile.CodeContent!);
        }
        else
        {
            analysisBuilder.AppendLine($"Skipping code for {codeFile.FileName}...");
        }
    }
}
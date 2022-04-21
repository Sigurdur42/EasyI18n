using Microsoft.Extensions.DependencyInjection;

namespace EasyI18n
{
    public static partial class EasyI18NExtensions
    {
        public static void AddEasyI18NEasyI18NGeneratorTests(this IServiceCollection _)
        {
            _.AddSingleton<LocalizeTestDataViewModel>();
        }
    }
}
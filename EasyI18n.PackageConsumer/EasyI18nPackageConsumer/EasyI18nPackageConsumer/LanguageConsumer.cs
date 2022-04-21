using EasyI18n;

namespace EasyI18nPackageConsumer
{
    public class LanguageConsumer
    {
        public LanguageConsumer(IEasyI18N easyI18N)
        {
            // This project consumes the nuget package
            // The analyzer is not run :(
            var localizedMessage = easyI18N.GetTestMessage();
        }
    }
}
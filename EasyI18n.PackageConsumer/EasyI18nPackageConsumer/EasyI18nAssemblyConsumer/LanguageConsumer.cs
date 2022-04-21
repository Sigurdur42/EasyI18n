using System;
using EasyI18n;

namespace EasyI18nAssemblyConsumer
{
    public class LanguageConsumer
    {
        public LanguageConsumer(IEasyI18N easyI18N)
        {
            // This project consumes the dll 
            // The analyzer is not run..

            var localizedMessage = easyI18N.GetTestMessage();
        }
    }
}

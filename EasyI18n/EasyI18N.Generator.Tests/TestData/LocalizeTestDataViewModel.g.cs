namespace EasyI18n
{
    public static class LocalizeTestData
    {
        public static string Gettest1(this IEasyI18N _) => _.Locale switch
        {
            "en" => @"test1-en",
            "de" => @"test1-de",
            _ => @"test1-de",
        };

        public static string Gettest2(this IEasyI18N _) => _.Locale switch
        {
            "en" => @"test2-en",
            "de" => @"test2-de",
            _ => @"test2-de",
        };

        public static string GetLangName(this IEasyI18N _) => _.Locale switch
        {
            "en" => @"English",
            "de" => @"Deutsch",
            _ => @"Deutsch",
        };

        public static string GetcomplexMessage(this IEasyI18N _) => _.Locale switch
        {
            "en" => @"A long
message with "" and öäü
and multiple lines",
            "de" => @"Eine lange Nachricht
mit tabs    und Anführungszeichen ""'
und mehreren Zeilen",
            _ => @"Eine lange Nachricht
mit tabs    und Anführungszeichen ""'
und mehreren Zeilen",
        };

    }
}
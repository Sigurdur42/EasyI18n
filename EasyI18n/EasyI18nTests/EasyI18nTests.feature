Feature: Ensure all localization Features work fine

    Scenario: Switching locale at runtime
        Given EasyI18n is initialized with locale 'de'
        And Messages from file 'LocalizeTestData.xml' are loaded
        Then The message 'LangName' is 'Deutsch'
        And The locale is changed to 'en'
        Then The message 'LangName' is 'English'

    Scenario: Multiple input files
        Given EasyI18n is initialized with locale 'de'
        And Messages from file 'LocalizeTestData.xml' are loaded
        And Messages from file 'LocalizeTestData2.xml' are loaded
        Then The message 'LangName' is 'Deutsch'
        Then The message 'file2:Test1' is 'file2-test1-de'
        And The locale is changed to 'en'
        Then The message 'file2:Test1' is 'file2-test1-en'
        
    Scenario: Multiple input files with duplicate keys
        Given EasyI18n is initialized with locale 'de'
        And Messages from file 'LocalizeTestData-duplicateKey1.xml' are loaded
        And Messages from file 'LocalizeTestData-duplicateKey2.xml' are loaded
        Then A Duplicate Key error occurs 

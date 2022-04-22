# EasyI18n
This project enables you to easily define localized messages, labels, etc. 
The core concept is based on XML files in which you define your messages for each supported language. 
Those messages then can easily be accessed in code in the currently configured language.

Features of EasyI18n are:

* Platform independend (just xml and C# code)
* Supports multiple XML files (you can put them in your localizable domain logic)
* Language can be switched at runtime
* A code generator can be used to generate high performance message access

# The language format
The localizable messages have to be defined in this simple XML format:
```
<messages>
    <message key="firstMessage">
        <locale lang="en">first message in English</locale>
        <locale lang="de">erster Text in Deutsch</locale>
    </message>
    <message key="second">
        <locale lang="en">second message in English</locale>
        <locale lang="de">zweiter Text in Deutsch</locale>
    </message>	
</messages>
```
You can have as many `message` nodes as you want. Each `message` can have a `locale` node per culture code (`en` and `de` in this example).
## Complex Messages
In case you need to define messages with XML reserved characters, new lines, etc , please use the `CDATA` notation:
```
<messages>
	<message key="complexMessage">
		<locale lang="en">
			<![CDATA[A long
message with " and öäü
and multiple lines]]>
		</locale>
		<locale lang="de">
			<![CDATA[Eine lange Nachricht
mit tabs    und Anführungszeichen "'
und mehreren Zeilen]]>
		</locale>
	</message>
</messages>
```

# Usage - General
## The nuget package 
The required classes are provided with the nuget package `EasyI18N`. Just reference the package start using the `IEasy18n` interface.

## Initializing 
You can initialize EasyI18n in one of two possible ways:
* Simply create a new instance of `EasyI18nContainer` and pass it to everywhere where you want to access the localized messages. 
* Via dependency injection. Just bind the container like this:
  ```
  // IServiceCollection service is created/passed above this code
  service.AddSingleton<IEasyI18N, EasyI18NContainer>();
  ```
  Then consume `IEasyI18n` via dependency injection wherever you require.

# Usage - Without generated Code
The messages need to be loaded at runtime from the XML files. This can be done using the `EasyI18NReader` class:
```
// given that IEasyI18n is accessible via the easyI18n
var reader = new EasyI18NReader(easyI18n);

var localizedMessageFile = new FileInfo("path to your xml file");
reader.Read(localizedMessageFile);
```

This `Read` call will
- load the XML file
- Register all messages in the given IEasyI18n instance
- Throw an exception in case of errors
    - The input cannot be found (`FileNotFoundException´)
    - At least one message key is not unique (`DuplicateKeyException´)
    
You now can access the localized messages by calling 
```
easyI18n.GetMessage("your message key");
```

# Usage - With generated Code
As the runtime lookup can get expensive when large amounts of messages are loaded, there is a more performant way of accessing the wanted message: Using the nuget package `EasyI18n.Generator`.

Add this package to every assembly where you define the messages in your XML file. The code generator will automatically generate extension methods to `IEasyI18n` in the form of `Get<your Message key>()`. The generated code consists of a generated switch statement which directly returns the wanted message in the current language. The expensive lookup is skipped.

This is the easiest way of using Easy18n. In order to do so, you need to set the compilation action of your localization XML files to `C# analyzer additional file`. When you now compile your assembly, the code is automatically generated.

Please note that the messages of the generated code is not added via the `EasyI18NReader` and therefore cannot be retrieved via the `GetMessage` method.

# Changing the Language
You can change the language at runtime by calling `easyI18n.SetLanguage("<your language code")`.

This will
- set the internal language to the wanted one
- Throw an exception if an unknown locale is set. This will be detected by calling `new CultureInfo()` where your locale is passed.
- trigger the .net event `LocaleChanged`. You can use this to react on changed locale and update your visible messages if you want.

# Formatting based on the current locale
You can pass the `FormatProvider` property to all formatting operations such as `.ToString()` to apply number format based on the selected locale.

# Version history
| Version | Date | Release Notes |
| ------- | ---- | ------------- |
| 1.0.0   | 22.04.2022 | Initial Release |
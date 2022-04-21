using System.Reflection;
using EasyI18n;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EasyI18nTests;

[Binding]
public class EasyI18NSteps
{
    EasyI18NContainer? _easyI18N;

    Exception? _lastException;

    [Given(@"EasyI18n is initialized with locale '(.*)'")]
    public void GivenEasyInIsInitializedWithLocale(string defaultLocale)
    {
        _easyI18N = new EasyI18NContainer(defaultLocale);
    }

    [Given(@"Messages from file '(.*)' are loaded")]
    public void GivenEasyInIsInitializedWithLocaleFromFile(string fileName)
    {
        try
        {
            var inputFile = new FileInfo(Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                fileName));

            var reader = new EasyI18NReader(_easyI18N!);
            reader.ReadFromFile(inputFile);
        }
        catch (Exception error)
        {
            _lastException = error;
        }
    }

    [Then(@"A Duplicate Key error occurs")]
    public void ThenADuplicateKeyErrorOccurs()
    {
        Assert.That(_lastException, Is.Not.Null, "Missing exception from last call");
        Assert.That(_lastException, Is.InstanceOf<DuplicateKeyException>(), "Wrong exception type");
    }

    [Then(@"The locale is changed to '(.*)'")]
    public void ThenTheLocaleIsChangedTo(string localeCode)
    {
        _easyI18N!.SetLocale(localeCode);
    }

    [Then(@"The message '(.*)' is '(.*)'")]
    public void ThenTheMessageIs(string messageId, string messageContent)
    {
        var found = _easyI18N!.GetText(messageId);
        Assert.That(found, Is.EqualTo(messageContent), $"key: {messageId} lang: {_easyI18N!.Locale}");
    }
}
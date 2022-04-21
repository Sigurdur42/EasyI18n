using EasyI18n;
using NUnit.Framework;

namespace EasyI18N.Tests;

[TestFixture]
public class SimpleGetMessageTest
{
    public void EnsureMessageTest()
    {
        var easyI18n = new EasyI18NContainer("de");
        var localizedMessage = easyI18n.GetTestMessage();
        Assert.That(localizedMessage, Is.EqualTo("Testnachricht"));
    }
}
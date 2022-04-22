using System.Reflection;
using System.Text;
using EasyI18N.Generator;
using EasyI18N.Generator.Tests;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EasyI18N.Tests
{
    [Binding]
    public class EasyI18NGeneratorSteps
    {
        private DirectoryInfo _dataDirectory = new DirectoryInfo(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "TestData"));

        private List<GeneratedCode> _generated = new();
        private GenerateCodeForXml _generator = new();

        StringBuilder _logBuilder = new StringBuilder();

        [Given(@"The manual generator is used")]
        public void GivenTheManualGeneratorIsUsed()
        {
            // nothing to do yet
        }

        [Then(@"Code file name is generated as '([^']*)'")]
        public void ThenCodeFileNameIsGeneratedAs(string fileName)
        {
            Assert.That(_generated[0].ExtensionClass.FileName, Is.EqualTo(fileName));
        }

        [Then(@"Code is generated as in '([^']*)'")]
        public void ThenCodeIsGeneratedAsIn(string inputFileName)
        {
            var finalName = Path.Combine(_dataDirectory.FullName, inputFileName);
            var reference = File.ReadAllText(finalName);
            NunitExtension.MultiLineAreEqual(_generated[0].ExtensionClass.CodeContent!, reference, "");
        }

        [Then(@"View model file name is generated as '([^']*)'")]
        public void ThenViewModelFileNameIsGeneratedAs(string fileName)
        {
            Assert.That(_generated[0].ViewModel.FileName, Is.EqualTo(fileName));
        }

        [Then(@"The view model is generated as in '([^']*)'")]
        public void ThenViewModelIsGeneratedAsIn(string inputFileName)
        {
            var finalName = Path.Combine(_dataDirectory.FullName, inputFileName);
            var reference = File.ReadAllText(finalName);

            NunitExtension.MultiLineAreEqual(_generated[0].ExtensionClass.CodeContent!, reference, "");
        }

        [When(@"Code has been generated successfully")]
        public void WhenCodeHasBeenGeneratedSuccessfully()
        {
            Assert.That(_generated[0].Success, Is.True);
        }

        [When(@"Code is generated for '([^']*)'")]
        public void WhenCodeIsGeneratedFor(string inputFileName)
        {
            var finalName = new FileInfo(Path.Combine(_dataDirectory.FullName, inputFileName));

            _generated.Add(_generator.GenerateCode(_logBuilder, finalName));
        }

        [When(@"Keys need to be translated")]
        public void WhenKeysNeedToBeTranslated(Table table)
        {
            foreach (var item in table.Rows)
            {
                var key = item["Key"];
                var value = item["Generated"];

                var generated = GenerateCodeForXml.MakeSafe(key);
                Assert.That(generated, Is.EqualTo(value));
            }
        }
    }
}
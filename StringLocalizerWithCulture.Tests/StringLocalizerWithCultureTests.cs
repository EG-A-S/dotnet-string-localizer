using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace StringLocalizerWithCulture.Tests
{
    public class StringLocalizerWithCultureTests
    {

        private readonly IStringLocalizerWithCultureFactory _factory;

        public StringLocalizerWithCultureTests() { 
            var services = new ServiceCollection()
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddLocalizationWithCulture(options => options.ResourcesPath = "TestData")
                .BuildServiceProvider();
            _factory = services.GetRequiredService<IStringLocalizerWithCultureFactory>();
        }

        private void TestGetString(string expected, string key, CultureInfo culture)
        {
            var actual = Translate(key, culture);
            actual.Value.Should().Be(expected);
        }

        private LocalizedString Translate(string key, CultureInfo culture)
        {
            var localizer = _factory.Create(typeof(MyClass), culture);
            var result = localizer[key];
            result.Should().NotBeNull();
            return result;
        }

        [Fact]
        public void GetString_Invariant()
        {
            TestGetString("Hello World", "Hello", CultureInfo.InvariantCulture);
        }

        [Fact]
        public void GetString_En()
        {
            TestGetString("Hello World", "Hello", CultureInfo.GetCultureInfo("en-US"));
        }

        [Fact]
        public void GetString_Fi()
        {
            TestGetString("Hei maailma", "Hello", CultureInfo.GetCultureInfo("fi-FI"));
        }

        [Fact]
        public void GetString_Null()
        {
            this.Invoking(self => self.Translate(null!, CultureInfo.GetCultureInfo("en-US")))
                .Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetString_Missing()
        {
            Translate("Missing", CultureInfo.GetCultureInfo("en-US")).ResourceNotFound.Should().BeTrue();
        }

        [Fact]
        public void GetString_Typed()
        {
            var localizer = _factory.Create<MyClass>(CultureInfo.InvariantCulture);
            localizer["Hello"].Value.Should().Be("Hello World");
        }

    }

    class MyClass
    {

    }
}
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
        private readonly CultureInfo _en = CultureInfo.GetCultureInfo("en-US");
        private readonly CultureInfo _fi = CultureInfo.GetCultureInfo("fi-FI");

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

        private void TestGetString(string expected, string key, string[] arguments, CultureInfo culture)
        {
            var localizer = _factory.Create(typeof(MyClass), culture);
            var actual = localizer[key, arguments];
            actual.Should().NotBeNull();
            actual.Value.Should().Be(expected);
        }

        [Fact]
        public void GetString_Invariant() => TestGetString("Hello World", "Hello", CultureInfo.InvariantCulture);

        [Fact]
        public void GetString_En() => TestGetString("Hello World", "Hello", _en);

        [Fact]
        public void GetString_Fi() => TestGetString("Hei maailma", "Hello", _fi);

        [Fact]
        public void GetString_Null() => 
            this.Invoking(self => self.Translate(null!, _en))
            .Should().Throw<ArgumentException>();

        [Fact]
        public void GetString_Missing() => Translate("Missing", _en).ResourceNotFound.Should().BeTrue();

        [Fact]
        public void GetString_Typed()
        {
            var localizer = _factory.Create<MyClass>(CultureInfo.InvariantCulture);
            localizer["Hello"].Value.Should().Be("Hello World");
        }

        [Fact]
        public void GetString_Param() => 
            TestGetString("Hello Mike", "HelloName", new[] { "Mike" }, CultureInfo.InvariantCulture);

        [Fact]
        public void GetString_Param_Fi() => 
            TestGetString("Hei, Mike", "HelloName", new[] { "Mike" }, _fi);
    }

    class MyClass
    {

    }
}
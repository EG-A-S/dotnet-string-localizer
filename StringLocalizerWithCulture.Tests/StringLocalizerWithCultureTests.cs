using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace StringLocalizerWithCulture.Tests
{
    public class StringLocalizerWithCultureTests
    {

        private readonly IStringLocalizerWithCultureFactory _factory;

        public StringLocalizerWithCultureTests() { 
            var services = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(new LoggerFactory())
                .AddLocalizationWithCulture()
                .BuildServiceProvider();
            _factory = services.GetRequiredService<IStringLocalizerWithCultureFactory>();
        }

        [Fact]
        public void GetString()
        {
            var localizer = _factory.Create(typeof(MyClass), CultureInfo.InvariantCulture);
            var actual = localizer["Hello"];
            actual.Should().Be("Hello World");
        }

    }

    class MyClass
    {

    }
}
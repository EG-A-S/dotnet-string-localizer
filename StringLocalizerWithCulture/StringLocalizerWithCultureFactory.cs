using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace StringLocalizerWithCulture
{
    public interface IStringLocalizerWithCultureFactory
    {
        IStringLocalizer Create(Type resourceSource, CultureInfo culture);
        IStringLocalizer<T> Create<T>(CultureInfo culture);
    }

    // Adapted from https://github.com/dotnet/aspnetcore/issues/7756
    // and https://gist.github.com/vaclavholusa-LTD/2a27d0bb0af5c07589cffbf1c2fff4f4
    internal sealed class StringLocalizerWithCultureFactory : ResourceManagerStringLocalizerFactory, IStringLocalizerWithCultureFactory
    {

        public StringLocalizerWithCultureFactory(
            IOptions<LocalizationOptions> localizationOptions,
            ILoggerFactory loggerFactory) 
            : base(localizationOptions, loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();
        private readonly ILoggerFactory _loggerFactory;
        private readonly ConcurrentDictionary<string, StringLocalizerWithCulture> _localizerCache 
            = new ConcurrentDictionary<string, StringLocalizerWithCulture>();

        public IStringLocalizer Create(Type resourceSource, CultureInfo culture) 
            => CreateResourceManagerStringLocalizer<object>(resourceSource, culture);

        public IStringLocalizer<T> Create<T>(CultureInfo culture) => CreateResourceManagerStringLocalizer<T>(typeof(T), culture);

        private StringLocalizerWithCulture<T> CreateResourceManagerStringLocalizer<T>(Type resourceSource,
            CultureInfo culture)
        {
            var typeInfo = resourceSource.GetTypeInfo();
            var baseName = GetResourcePrefix(typeInfo);
            var assembly = typeInfo.Assembly;
            return (StringLocalizerWithCulture<T>)_localizerCache.GetOrAdd(baseName, _ => CreateResourceManagerStringLocalizer<T>(assembly, baseName, culture));
        }

        private StringLocalizerWithCulture<T> CreateResourceManagerStringLocalizer<T>(
            Assembly assembly,
            string baseName,
            CultureInfo culture)
        {
            var resources = new ResourceManager(baseName, assembly);
            return new StringLocalizerWithCulture<T>(resources, assembly, baseName, _resourceNamesCache,
                _loggerFactory.CreateLogger<ResourceManagerStringLocalizer>(),
                culture);
        }
    }
}

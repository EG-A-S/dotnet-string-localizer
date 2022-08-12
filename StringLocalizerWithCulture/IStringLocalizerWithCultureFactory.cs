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
        {
            if (resourceSource == null)
            {
                throw new ArgumentNullException(nameof(resourceSource));
            }

            var typeInfo = resourceSource.GetTypeInfo();
            var baseName = GetResourcePrefix(typeInfo);
            var assembly = typeInfo.Assembly;

            return _localizerCache.GetOrAdd(baseName, _ => CreateResourceManagerStringLocalizer(assembly, baseName, culture));
        }

        private StringLocalizerWithCulture CreateResourceManagerStringLocalizer(
            Assembly assembly,
            string baseName,
            CultureInfo culture)
        {
            return new StringLocalizerWithCulture(
                new ResourceManager(baseName, assembly),
                culture
                //assembly,
                //baseName,
                //_resourceNamesCache,
                //_loggerFactory.CreateLogger<ResourceManagerStringLocalizer>()
            );
        }
    }
}

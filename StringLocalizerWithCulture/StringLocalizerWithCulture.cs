using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace StringLocalizerWithCulture
{
    internal class StringLocalizerWithCulture : ResourceManagerStringLocalizer
    {

        public StringLocalizerWithCulture(ResourceManager resourceManager,
            Assembly resourceAssembly,
            string baseName,
            IResourceNamesCache resourceNamesCache,
            ILogger logger, CultureInfo culture)
            : base(resourceManager, resourceAssembly, baseName, resourceNamesCache, logger)
        {
            _baseName = baseName;
            _culture = culture;
        }

        private readonly CultureInfo _culture;
        private readonly string _baseName;

        public override LocalizedString this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException("Translation key should not be null or empty", nameof(name));
                var value = GetStringSafely(name, _culture);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _baseName);
            }
        }

        public override LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetStringSafely(name, _culture);
                var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: _baseName);
            }
        }
    }

    internal class StringLocalizerWithCulture<T> : StringLocalizerWithCulture, IStringLocalizer<T>
    {
        public StringLocalizerWithCulture(ResourceManager resourceManager,
            Assembly resourceAssembly,
            string baseName,
            IResourceNamesCache resourceNamesCache,
            ILogger logger, CultureInfo culture) : base(resourceManager, resourceAssembly, baseName, resourceNamesCache, logger, culture)
        {
        }
    }

}

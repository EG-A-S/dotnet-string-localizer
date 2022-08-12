using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace StringLocalizerWithCulture
{
    internal class StringLocalizerWithCulture : IStringLocalizer
    {

        public StringLocalizerWithCulture(ResourceManager resources, CultureInfo culture)
        {
            _resources = resources;
            _culture = culture;
        }

        private readonly CultureInfo _culture;
        private readonly ResourceManager _resources;

        public LocalizedString this[string name] => new LocalizedString(name, _resources.GetString(name, _culture));

        public LocalizedString this[string name, params object[] arguments]
            => new LocalizedString(name, _resources.GetString(name, _culture));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }

    internal class StringLocalizerWithCulture<T> : StringLocalizerWithCulture, IStringLocalizer<T>
    {
        public StringLocalizerWithCulture(ResourceManager resources, CultureInfo culture) : base(resources, culture)
        {
        }
    }

}

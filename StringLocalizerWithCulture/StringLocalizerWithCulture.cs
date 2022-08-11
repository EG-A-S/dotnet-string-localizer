using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace StringLocalizerWithCulture
{
    internal class StringLocalizerWithCulture : IStringLocalizer
    {

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


}

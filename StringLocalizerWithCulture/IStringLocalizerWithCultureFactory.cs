using Microsoft.Extensions.Localization;
using System;
using System.Globalization;

namespace StringLocalizerWithCulture
{
    public interface IStringLocalizerWithCultureFactory
    {
        IStringLocalizer Create(Type type, CultureInfo culture);
    }

    public class StringLocalizerWithCultureFactory : IStringLocalizerWithCultureFactory
    {
        public IStringLocalizer Create(Type type, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

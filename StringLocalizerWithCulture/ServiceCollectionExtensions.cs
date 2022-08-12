using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;

namespace StringLocalizerWithCulture
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizationWithCulture(this IServiceCollection services, Action<LocalizationOptions> setupAction = null)
        {
            services.AddOptions();

            services.TryAddSingleton<IStringLocalizerWithCultureFactory, StringLocalizerWithCultureFactory>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizerWithCulture<>));
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }
    }
}

# .NET string localizer with culture

Implements [`IStringLocalizer`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.localization.istringlocalizer)
that is bound to specific culture.

See https://github.com/dotnet/aspnetcore/issues/7756 for context.

```csharp
var services = new ServiceCollection()
    .AddLocalizationWithCulture(options => options.ResourcesPath = "Resources")
    .BuildServiceProvider();

var localizerFactory = services.GetRequiredService<IStringLocalizerWithCultureFactory>();
var localizer = localizerFactory.Create(typeof(MyClass), CultureInfo.GetCultureInfo("en-US"));
var translated = localizer["Hello"];
```
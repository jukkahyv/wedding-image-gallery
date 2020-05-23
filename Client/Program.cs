using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Blazored.LocalStorage;
using WeddingImageGallery.Shared;
using Microsoft.JSInterop;
using Microsoft.Extensions.Localization;
using WeddingImageGallery.Client.Pages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WeddingImageGallery.Client {
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
				.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
				.AddBlazoredLocalStorage()
				.AddSingleton<PasswordCheckContext>()
				.AddLocalization()
				// Without these, Unhandled exception rendering component: A suitable constructor for type 'Microsoft.Extensions.Localization.StringLocalizer`1[WeddingImageGallery.Client.Pages.LoginForm]' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.
				.AddSingleton(sp => new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions { ResourcesPath = "." }), sp.GetRequiredService<ILoggerFactory>()))
				.AddSingleton(sp => new StringLocalizer<LoginForm>(sp.GetRequiredService<IStringLocalizerFactory>()))
				.AddSingleton(sp => new StringLocalizer<App>(sp.GetRequiredService<IStringLocalizerFactory>()))
				;

			var host = builder.Build();
			var localStorage = host.Services.GetRequiredService<ILocalStorageService>();

			var language = await localStorage.GetItemAsync<Language?>("Language") ?? await GetBrowserLanguage(host);
			var culture = new CultureInfo(language == Language.Finnish ? "fi-FI" : "en-US");
			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;

			builder.Services.AddSingleton(new LanguageContext(language));
			host = builder.Build();

			await host.RunAsync();
        }

		private static Language GetLanguage(CultureInfo culture) {
			return culture?.TwoLetterISOLanguageName == "fi" ? Language.Finnish : Language.English;
		}

		private static async Task<Language> GetBrowserLanguage(WebAssemblyHost host) {
			var browserLocale = (await host.Services.GetRequiredService<IJSRuntime>().InvokeAsync<string>("js.getBrowserLocale"))?.ToLowerInvariant();
			return (browserLocale == "fi" || browserLocale == "fi-fi") ? Language.Finnish : Language.English;
		}

	}

}

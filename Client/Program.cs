using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Blazored.LocalStorage;
using WeddingImageGallery.Shared;
using Microsoft.JSInterop;

namespace WeddingImageGallery.Client {
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services
				.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
				.AddLocalization()
				.AddBlazoredLocalStorage()
				.AddSingleton<PasswordCheckContext>();

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

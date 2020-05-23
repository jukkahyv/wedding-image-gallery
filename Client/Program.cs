using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Blazored.LocalStorage;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Client {
	public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddLocalization();
			builder.Services.AddBlazoredLocalStorage();

			var host = builder.Build();
			var localStorage = host.Services.GetRequiredService<ILocalStorageService>();

			var language = await localStorage.GetItemAsync<Language?>("Language");
			if (language != null) {
				var culture = new CultureInfo(language == Language.Finnish ? "fi-FI" : "en-US");
				CultureInfo.DefaultThreadCurrentCulture = culture;
				CultureInfo.DefaultThreadCurrentUICulture = culture;
			}

			builder.Services.AddSingleton(new LanguageContext(language ?? Language.English));
			host = builder.Build();

			await host.RunAsync();
        }
    }
}

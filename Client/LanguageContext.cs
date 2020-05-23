using System;
using System.Globalization;
using System.Threading.Tasks;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Client {
	public class LanguageContext {

		public LanguageContext() {
			Language = CultureInfo.DefaultThreadCurrentUICulture?.Name == "fi-FI" ? Language.Finnish : Language.English;
		}

		public Language Language { get; private set; }
		public Func<Language, Task> LanguageChanged { get; set; }
		public async Task SetLanguage(Language language) {
			Language = language;
			await LanguageChanged(language);
		}
	}
}

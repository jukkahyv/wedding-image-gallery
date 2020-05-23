using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Client {
	public class LanguageContext {

		public LanguageContext(Language language) {
			Language = language;
			//Language = CultureInfo.DefaultThreadCurrentUICulture?.Name == "fi-FI" ? Language.Finnish : Language.English;
		}

		public Language Language { get; private set; }
	}
}

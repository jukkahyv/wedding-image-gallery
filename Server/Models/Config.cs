using System.Collections.Generic;

namespace WeddingImageGallery.Server.Models {
	public class Config {
		public string ImageFolder { get; set; }
		public string Password { get; set; }
		public Dictionary<string, string> GalleryNamesEn { get; set; }
		public Dictionary<string, string> GalleryNamesFi { get; set; }
	}
}

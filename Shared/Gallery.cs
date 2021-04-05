namespace WeddingImageGallery.Shared {

	public class Gallery {

		public Gallery() { }

		public Gallery(string path, string name) {
			Name = name;
			Path = path;
		}

		public string Name { get; set; }
		public string Path { get; set; }

		public override string ToString() => $"{Name} ({Path})";

	}

}

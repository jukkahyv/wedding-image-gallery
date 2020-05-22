
namespace WeddingImageGallery.Shared
{
    public class ImageProperties
    {
        public ImageProperties()
        {
        }

        public ImageProperties(string fullUrl, string name)
        {
            FullUrl = fullUrl;
			Name = name;
        }

        public string FullUrl { get; set; }
		public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeddingImageGallery.Server.Models;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Server.Controllers
{
    [ApiController]
    [Route("/api/galleries")]
    public class ImageController : ControllerBase
    {

        public ImageController(IWebHostEnvironment env, IOptions<Config> config)
        {
			FilesRoot = config.Value?.ImageFolder ?? (env.WebRootPath + "/images");
        }

        private string FilesRoot { get; }

        private ImageProperties GetImageProperties(string filePath)
        {
            var requestBase = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{Startup.ImageRequestPath.Value}/";
            var relativePath = Path.GetRelativePath(FilesRoot, filePath);
            var url = requestBase + relativePath.Replace('\\', '/');
            return new ImageProperties(url);
        }

		private Gallery GetGallery(string directory) {
			var path = Path.GetFileName(directory);
			return new Gallery(path, path);
		}

		public IEnumerable<Gallery> GetGalleries() {
			var directories = Directory.GetDirectories(FilesRoot);
			return directories.Select(dir => GetGallery(dir));
		}

		[Route("/api/galleries/{gallery}/images")]
        public IEnumerable<ImageProperties> GetImages(string gallery, int skip = 0) {

            var extensions = new[] { ".jpg", ".png" };

            var images = Directory.EnumerateFiles(Path.Combine(FilesRoot, @$"{gallery}"), "*.*", SearchOption.TopDirectoryOnly)
                .Where(i => extensions.Contains(Path.GetExtension(i)))
                .OrderBy(i => i)
                .Skip(skip)
                .Take(Constants.PageSize);

            return images.Select(GetImageProperties);

        }

    }
}

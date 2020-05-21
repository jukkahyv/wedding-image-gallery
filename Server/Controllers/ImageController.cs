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
    [Route("/api/images")]
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

        public IEnumerable<ImageProperties> GetImages(int skip = 0)
        {

            var extensions = new[] { ".jpg", ".png" };

            var images = Directory.EnumerateFiles(FilesRoot, "*.*", SearchOption.AllDirectories)
                .Where(i => extensions.Contains(Path.GetExtension(i)))
                .OrderBy(i => i)
                .Skip(skip)
                .Take(Constants.PageSize);

            return images.Select(GetImageProperties);

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Server.Controllers
{
    [ApiController]
    [Route("/api/images")]
    public class ImageController : ControllerBase
    {

        public ImageController(IWebHostEnvironment env)
        {
            WebRoot = env.WebRootPath;
        }

        private string RequestBase { get; }
        private string WebRoot { get; }

        private ImageProperties GetImageProperties(string filePath)
        {
            var requestBase = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/";
            var relativePath = Path.GetRelativePath(WebRoot, filePath);
            var url = requestBase + relativePath.Replace('\\', '/');
            return new ImageProperties(url);
        }

        public IEnumerable<ImageProperties> GetImages()
        {

            var extensions = new[] { ".jpg", ".png" };

            var images = Directory.EnumerateFiles(WebRoot + "\\images", "*.*", SearchOption.AllDirectories)
                .Where(i => extensions.Contains(Path.GetExtension(i)));

            return images.Select(GetImageProperties);

        }
    }
}

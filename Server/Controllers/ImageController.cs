using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        private string Folder { get; } = @"C:\Users\Jukka\Pictures\CD";

        private async Task<string> GetViewUrl(string file)
        {

        }

        public async Task<IEnumerable<ImageProperties>> GetImages()
        {
            var images = Directory.GetFiles(Folder, "*.jpg;*.png");

        }
    }
}

using FluentAssertions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using WeddingImageGallery.Server.Controllers;
using WeddingImageGallery.Server.Models;
using WeddingImageGallery.Test.TestSupport;
using Xunit;

namespace WeddingImageGallery.Test {

	/// <summary>
	/// Tests for <see cref="ImageController"/>.
	/// </summary>
	public class ImageControllerTests {

		public ImageControllerTests() {
			var config = new Config { ImageFolder = "..\\..\\..\\TestData", GalleryNamesEn = new Dictionary<string, string> { { "MyGallery", "My gallery" } } };
			_controller = new ImageController(new MockWebHostEnvironment(), Options.Create(config));
		}

		private readonly ImageController _controller;

		[Fact]
		public void GetGalleries() {

			var galleries = _controller.GetGalleries(Shared.Language.English);

			galleries.Should().Contain(gallery => gallery.Name == "My gallery")
				.Which.Path.Should().Be("MyGallery");

		}

	}

}

using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using WeddingImageGallery.Server;
using Xunit;
using System.Net.Http;
using WeddingImageGallery.Shared;
using FluentAssertions;

namespace WeddingImageGallery.Test {

	/// <summary>
	/// ASP.NET integration tests
	/// </summary>
	public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>> {

		private readonly WebApplicationFactory<Startup> factory;

		public IntegrationTests(WebApplicationFactory<Startup> factory) {
			this.factory = factory;
		}

		[Fact]
		public async Task GetGalleries() {

			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/galleries");

			response.EnsureSuccessStatusCode();
			var galleries = await response.Content.ReadAsAsync<Gallery[]>();

			galleries.Should().NotBeEmpty();

			// TODO: refactor ImageController to use virtual filesystem + add more asserts

		}

	}

}

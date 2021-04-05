using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using WeddingImageGallery.Server;
using Xunit;

namespace WeddingImageGallery.Test {

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

		}

	}

}

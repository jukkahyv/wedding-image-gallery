using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeddingImageGallery.Server.Models;
using WeddingImageGallery.Shared;

namespace WeddingImageGallery.Server.Controllers {

	[Route("api/[controller]")]
	public class UserController : ControllerBase {

		public UserController(IOptions<Config> config) {
			_config = config.Value;
		}

		private readonly Config _config;

		[HttpGet("check-password")]
		public PasswordCheckResult CheckPassword(string password) {
			return new PasswordCheckResult {
				PasswordValid = string.Equals(_config.Password, password, StringComparison.InvariantCultureIgnoreCase)
			};
		}

	}

}

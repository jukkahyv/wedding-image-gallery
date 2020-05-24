using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;
using System;
using System.Threading.Tasks;

namespace WeddingImageGallery.Server {

	/// <summary>
	/// Returns images stored in the local physical file system.
	/// https://github.com/SixLabors/ImageSharp.Web/blob/master/src/ImageSharp.Web/Providers/PhysicalFileSystemProvider.cs
	/// </summary>
	public class CustomPhysicalFileSystemProvider : IImageProvider {

		/// <summary>
		/// The file provider abstraction.
		/// </summary>
		private readonly IFileProvider fileProvider;

		/// <summary>
		/// Contains various format helper methods based on the current configuration.
		/// </summary>
		private readonly FormatUtilities formatUtilities;

		/// <summary>
		/// Initializes a new instance of the <see cref="PhysicalFileSystemProvider"/> class.
		/// </summary>
		/// <param name="environment">The environment used by this middleware.</param>
		/// <param name="formatUtilities">Contains various format helper methods based on the current configuration.</param>
		public CustomPhysicalFileSystemProvider(
			IFileProvider fileProvider,
			FormatUtilities formatUtilities) {

			this.fileProvider = fileProvider;
			this.formatUtilities = formatUtilities;
			Match = IsMatch;
		}

		/// <inheritdoc/>
		public ProcessingBehavior ProcessingBehavior { get; } = ProcessingBehavior.CommandOnly;

		public PathString RequestPath { get; set; }

		/// <inheritdoc/>
		public Func<HttpContext, bool> Match { get; set; }

		private bool IsMatch(HttpContext ctx) => ctx.Request.Path.StartsWithSegments(RequestPath);

		/// <inheritdoc/>
		public bool IsValidRequest(HttpContext context) => this.formatUtilities.GetExtensionFromUri(context.Request.GetDisplayUrl()) != null;

		/// <inheritdoc/>
		public Task<IImageResolver> GetAsync(HttpContext context) {

			// https://github.com/dotnet/aspnetcore/blob/master/src/Middleware/StaticFiles/src/StaticFileMiddleware.cs
			if (!TryMatchPath(context, RequestPath, false, out var subPath))
				return Task.FromResult<IImageResolver>(null);

			// Path has already been correctly parsed before here.
			IFileInfo fileInfo = this.fileProvider.GetFileInfo(subPath);

			// Check to see if the file exists.
			if (!fileInfo.Exists) {
				return Task.FromResult<IImageResolver>(null);
			}

			var metadata = new ImageMetadata(fileInfo.LastModified.UtcDateTime);
			return Task.FromResult<IImageResolver>(new PhysicalFileSystemResolver(fileInfo, metadata));
		}

		// https://github.com/dotnet/aspnetcore/blob/master/src/Middleware/StaticFiles/src/Helpers.cs
		internal static bool TryMatchPath(HttpContext context, PathString matchUrl, bool forDirectory, out PathString subpath) {
			var path = context.Request.Path;

			if (forDirectory && !path.Value.EndsWith("/")) {
				path += new PathString("/");
			}

			if (path.StartsWithSegments(matchUrl, out subpath)) {
				return true;
			}
			return false;
		}
	}

}

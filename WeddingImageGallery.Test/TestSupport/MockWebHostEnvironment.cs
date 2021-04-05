using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;

namespace WeddingImageGallery.Test.TestSupport {

	public class MockWebHostEnvironment : IWebHostEnvironment {
		public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string WebRootPath { get; set; }
		public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}

}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Web.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using WeddingImageGallery.Server.Models;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.Providers;

namespace WeddingImageGallery.Server {
	public class Startup
    {

		public static readonly PathString ImageRequestPath = new PathString("/images");

		public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }
		private IFileProvider fileProvider;

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {

			services.Configure<Config>(Configuration);
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddImageSharp()
				.RemoveProvider<PhysicalFileSystemProvider>()
				.AddProvider(serv => new CustomPhysicalFileSystemProvider(fileProvider, serv.GetRequiredService<SixLabors.ImageSharp.Web.FormatUtilities>()) {
					RequestPath = ImageRequestPath
				});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<Config> config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseImageSharp();
            app.UseStaticFiles();

			fileProvider = new PhysicalFileProvider(config.Value.ImageFolder);
			app.UseStaticFiles(new StaticFileOptions() {
				FileProvider = fileProvider,
				RequestPath = ImageRequestPath
			});

			app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}

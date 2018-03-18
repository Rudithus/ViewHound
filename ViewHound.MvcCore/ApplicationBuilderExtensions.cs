using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ViewHound
{
    public static class ApplicationBuilderExtensions
    {
        public static IMvcBuilder AddHound(this IMvcBuilder mvcBuilder)
        {
            var engines = mvcBuilder.AddViewOptions(options =>
            options.ViewEngines.Insert(0, new TrackerViewEngine()));

            return mvcBuilder;
        }

        public static IApplicationBuilder UseHound(this IApplicationBuilder app)
        {
            app.UseMiddleware<Hound>();

            return app;
        }
    }
}

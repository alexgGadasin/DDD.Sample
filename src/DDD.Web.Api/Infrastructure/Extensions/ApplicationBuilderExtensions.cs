using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DDD.Web.Api.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app, Func<HttpContext, bool> condition, Action<IApplicationBuilder> configuration)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            var builder = app.New();
            configuration(builder);
            return app.Use(next =>
            {
                builder.Run(next);
                var branch = builder.Build();
                return context => condition(context) ? branch(context) : next(context);
            });
        }

        public static IApplicationBuilder ConfigureWhen(this IApplicationBuilder app, bool condition, Action<IApplicationBuilder> configuration)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (condition)
            {
                configuration(app);
            }
            return app;
        }
    }
}
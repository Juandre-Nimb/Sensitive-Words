namespace SensitiveWordsClean.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sensitive Words Clean API v1");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = "Sensitive Words Clean API";
            });
        }

        return app;
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors("Development");
        }
        else
        {
            app.UseCors("AllowWebApp");
        }

        return app;
    }
}

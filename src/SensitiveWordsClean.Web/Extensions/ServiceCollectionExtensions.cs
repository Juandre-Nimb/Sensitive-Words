namespace SensitiveWordsClean.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();

        services.AddHttpClient("SensitiveWordsAPI", client =>
        {
            var baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7203/api/";
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", "SensitiveWordsClean.Web/1.0");
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            // Skip SSL validation for localhost development
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        });

        return services;
    }

    public static IServiceCollection AddSecurityHeaders(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.SuppressXFrameOptionsHeader = false;
        });

        return services;
    }
}

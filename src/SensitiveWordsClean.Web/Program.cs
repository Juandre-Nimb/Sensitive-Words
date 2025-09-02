using SensitiveWordsClean.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddSecurityHeaders();

var app = builder.Build();

app.UseCustomExceptionHandling(app.Environment);
app.UseHttpsRedirection();
app.UseSecurityHeaders();
app.UseStaticFilesWithCaching(app.Environment);
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

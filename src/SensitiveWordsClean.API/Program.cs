using SensitiveWordsClean.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApiDocumentation();
builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseApiDocumentation(app.Environment);
app.UseHttpsRedirection();
app.UseCorsPolicy(app.Environment);
app.UseAuthorization();
app.MapControllers();

app.Run();

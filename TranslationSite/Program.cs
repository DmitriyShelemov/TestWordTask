using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using TranslationSite.Infrastructure;
using WordParsing.Logic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITranslationService, TranslationService>(
    (provider) => new TranslationService(
        builder.Configuration.GetValue<string>("YaKey"), 
        builder.Configuration.GetValue<string>("YaFolderId")));

builder.Services.AddScoped<IAsposeWrapper, AsposeWrapper>();
builder.Services.AddScoped<IWordParsingService, WordParsingService>();
builder.Services.AddScoped<IRequestValidationHelper, RequestValidationHelper>();
builder.Services.AddScoped<ITemporaryFileStorage, TemporaryFileStorage>();

builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        spa.UseReactDevelopmentServer(npmScript: "start");
    }
});

app.Run();

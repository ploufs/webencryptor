var builder = WebApplication.CreateBuilder(args);

// ajout openApi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "webencryptor";
        document.Info.Description = "";
        document.Info.Version = "v1";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// ajout openApi endpoint (/openapi/v1.json)
app.MapOpenApi();

app.MapGet("/", () => "Hello World!");

app.Run();

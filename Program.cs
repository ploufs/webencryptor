using PgpCore;

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

app.MapGet("/PGPEncrypt", async (string filenamePublicKey,string text) =>
{
    // Load keys
    var publicKey = File.ReadAllText($"PGPPublicKey\\{filenamePublicKey}");
    var encryptionKeys = new EncryptionKeys(publicKey);

    // Encrypt
    var pgp = new PGP(encryptionKeys);
    return  await pgp.EncryptAsync(text);
});

app.Run();

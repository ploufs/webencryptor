using Microsoft.AspNetCore.Mvc;
using PgpCore;
using Scalar.AspNetCore;
using webencryptor;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// aller chercher les configuration de php dans appsettings
var pgpConfig = configuration.GetRequiredSection("Pgp").Get<PgpConfig>();

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

// ajouter les configurations de Scalar
builder.Services.AddOptions<ScalarOptions>().BindConfiguration("Scalar");

var app = builder.Build();

// ajout openApi endpoint (/openapi/v1.json)
app.MapOpenApi();

// ajouter scalar (/scalar/v1)
// source : https://github.com/scalar/scalar/blob/main/documentation/integrations/dotnet.md , https://github.com/scalar/scalar/blob/main/packages/scalar.aspnetcore/README.md)
app.MapScalarApiReference();

// redirect to scalar documentation
app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.MapPost("/PGPEncrypt", async ([FromForm] string filenamePublicKey, [FromForm] string text) =>
{
    // Load keys
    var publicKeyFilename = Path.Combine("PGPPublicKey", filenamePublicKey);
    if (!File.Exists(publicKeyFilename)) throw new FileNotFoundException("publickey not found", publicKeyFilename);
    var publicKey = File.ReadAllText(publicKeyFilename);
    var encryptionKeys = new EncryptionKeys(publicKey);

    // Encrypt
    var pgp = new PGP(encryptionKeys);

    return await pgp.EncryptAsync(text, headers: pgpConfig.Headers);
}).DisableAntiforgery();

app.MapPost("/PGPEncryptFromProtonEmail", async ([FromForm] string protonEmail, [FromForm] string text) =>
{
    var publicKey = string.Empty;
    // download proton plubic key (source: https://proton.me/support/download-public-private-key)
    using (var client = new HttpClient())
    {
        publicKey = await client.GetStringAsync($"https://mail-api.proton.me/pks/lookup?op=get&search={protonEmail}");
    }

    var encryptionKeys = new EncryptionKeys(publicKey);

    // Encrypt
    var pgp = new PGP(encryptionKeys);
    return await pgp.EncryptAsync(text, headers: pgpConfig.Headers);
}).DisableAntiforgery();

app.Run();
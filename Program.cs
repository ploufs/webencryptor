using PgpCore;
using System.Net;
using System;
using Scalar.AspNetCore;

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

// ajouter scalar (/scalar/v1)
// source : https://github.com/scalar/scalar/blob/main/documentation/integrations/dotnet.md , https://github.com/scalar/scalar/blob/main/packages/scalar.aspnetcore/README.md)
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("webencryptor")
        .WithSidebar(true);

});

// redirect to scalar documentation
app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.MapGet("/helloword", () => "Hello World!");

app.MapGet("/PGPEncrypt", async (string filenamePublicKey,string text) =>
{
    // Load keys
    var publicKey = File.ReadAllText($"PGPPublicKey\\{filenamePublicKey}");
    var encryptionKeys = new EncryptionKeys(publicKey);

    // Encrypt
    var pgp = new PGP(encryptionKeys);
    return  await pgp.EncryptAsync(text);
});

app.MapGet("/PGPEncryptFromProtonEmail", async (string protonEmail, string text) =>
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
    return await pgp.EncryptAsync(text);
});

app.Run();

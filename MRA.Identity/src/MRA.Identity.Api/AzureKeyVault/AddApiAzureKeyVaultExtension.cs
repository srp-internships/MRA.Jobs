using Azure.Identity;
using System.Security.Cryptography.X509Certificates;

namespace MRA.Identity.Api.AzureKeyVault;

public static class WebApplicationBuilderExtension
{ 
    public static void ConfigureAzureKeyVault(this WebApplicationBuilder builder)
    {
            using var x509Store = new X509Store(StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);

            var thumbprint = builder.Configuration["AzureKeyVault:AzureADCertThumbprint"];

            var certificate = x509Store.Certificates
                .Find(
                    X509FindType.FindByThumbprint,
                    thumbprint,
                    validOnly: false)
                .OfType<X509Certificate2>()
                .Single();

            builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{builder.Configuration["AzureKeyVault:KeyVaultName"]}.vault.azure.net/"),
                new ClientCertificateCredential(
                    builder.Configuration["AzureKeyVault:AzureADDirectoryId"],
                    builder.Configuration["AzureKeyVault:AzureADApplicationId"],
                    certificate), new PrefixKeyVaultSecretManager());
    }
}

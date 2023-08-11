using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace MRA.Jobs.Web.AzureKeyVault;

public class PrefixKeyVaultSecretManager : KeyVaultSecretManager
{
    public override string GetKey(KeyVaultSecret secret)
    {
        if (secret.Name.StartsWith("MRAJobs"))
            return secret.Name.Replace("MRAJobs-", "").Replace("-", ConfigurationPath.KeyDelimiter);
        
        return secret.Name.Replace("-", ConfigurationPath.KeyDelimiter);
    }
}
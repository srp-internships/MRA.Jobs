using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace MRA.Jobs.Web.AzureKeyVault;

public class PrefixKeyVaultSecretManager : KeyVaultSecretManager
{
    const string jobsProjectName = "MRAJobs";
    public override string GetKey(KeyVaultSecret secret)
    {
        if (secret.Name.StartsWith(jobsProjectName))
            return secret.Name.Replace("MRAJobs-", "").Replace("-", ConfigurationPath.KeyDelimiter);
        
        return secret.Name.Replace("-", ConfigurationPath.KeyDelimiter);
    }
}

#r "nuget: Azure.Identity, *"
#r "nuget: Azure.Security.KeyVault.Certificates, *"
#r "nuget: Azure.Security.KeyVault.Secrets, *"

using System.Security.Cryptography.X509Certificates;

using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;

var certificate = new X509Certificate2(@"");
var localFilePath = @"D:\your\path\file.pfx";

// Create a new instance of the client certificate credential
var credential = new ClientCertificateCredential(
    "",
    "",
    certificate,
    new ClientCertificateCredentialOptions
    {
        // This must be enabled for SNI auth
        SendCertificateChain = true
    });

// test the credential by acquiring a token
var token = credential.GetToken(new TokenRequestContext(new[] { "https://graph.microsoft.com/.default" }));
Console.WriteLine(token.Token);

// test the credential by accessing Key Vault
var akvUrl = @"https://<AKV_URL>.vault.azure.net/";

// Create SecretClient and CertificateClient
var client = new SecretClient(new Uri(akvUrl), credential);
var certClient = new CertificateClient(new Uri(akvUrl), credential);

// retrieve secrets
var keysInAkv = new string[]
{
    "key1",
    "key2",
};

foreach (var key in keysInAkv)
{
    KeyVaultSecret secret = client.GetSecret(key);
    Console.WriteLine($"Secret: {secret.Name} = {secret.Value}");
}

// retrieve a certificate
var cert = certClient.DownloadCertificate("<CERT_NAME>");
File.WriteAllBytes(localFilePath, cert.Value.Export(X509ContentType.Pfx));

Console.WriteLine($"Certificate saved to {localFilePath}");

Console.WriteLine("Hello, World!");


/// <summary>
/// Load a certificate (with private key) from Azure Key Vault
///
/// Getting a certificate with private key is a bit of a pain, but the code below solves it.
/// 
/// Get the private key for Key Vault certificate
/// https://github.com/heaths/azsdk-sample-getcert
/// 
/// See also these GitHub issues: 
/// https://github.com/Azure/azure-sdk-for-net/issues/12742
/// https://github.com/Azure/azure-sdk-for-net/issues/12083
/// </summary>
/// <param name="config"></param>
/// <param name="certificateName"></param>
/// <returns></returns>
public static X509Certificate2 LoadCertificate(IConfiguration config, string certificateName)
{
    string vaultUrl = config["Vault:Url"] ?? "";
    string clientId = config["Vault:ClientId"] ?? "";
    string tenantId = config["Vault:TenantId"] ?? "";
    string secret = config["Vault:ClientSecret"] ?? "";

    Console.WriteLine($"Loading certificate '{certificateName}' from Azure Key Vault");

    var credentials = new ClientSecretCredential(tenantId: tenantId, clientId: clientId, clientSecret: secret);
    var certClient = new CertificateClient(new Uri(vaultUrl), credentials);
    var secretClient = new SecretClient(new Uri(vaultUrl), credentials);

    var cert = GetCertificateAsync(certClient, secretClient, certificateName);

    Console.WriteLine("Certificate loaded");
    return cert;
}


/// <summary>
/// Helper method to get a certificate
/// 
/// Source https://github.com/heaths/azsdk-sample-getcert/blob/master/Program.cs
/// </summary>
/// <param name="certificateClient"></param>
/// <param name="secretClient"></param>
/// <param name="certificateName"></param>
/// <returns></returns>
private static X509Certificate2 GetCertificateAsync(CertificateClient certificateClient,
                                                        SecretClient secretClient,
                                                        string certificateName)
{

    KeyVaultCertificateWithPolicy certificate = certificateClient.GetCertificate(certificateName);

    // Return a certificate with only the public key if the private key is not exportable.
    if (certificate.Policy?.Exportable != true)
    {
        return new X509Certificate2(certificate.Cer);
    }

    // Parse the secret ID and version to retrieve the private key.
    string[] segments = certificate.SecretId.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
    if (segments.Length != 3)
    {
        throw new InvalidOperationException($"Number of segments is incorrect: {segments.Length}, URI: {certificate.SecretId}");
    }

    string secretName = segments[1];
    string secretVersion = segments[2];

    KeyVaultSecret secret = secretClient.GetSecret(secretName, secretVersion);

    // For PEM, you'll need to extract the base64-encoded message body.
    // .NET 5.0 preview introduces the System.Security.Cryptography.PemEncoding class to make this easier.
    if ("application/x-pkcs12".Equals(secret.Properties.ContentType, StringComparison.InvariantCultureIgnoreCase))
    {
        byte[] pfx = Convert.FromBase64String(secret.Value);
        return new X509Certificate2(pfx);
    }

    throw new NotSupportedException($"Only PKCS#12 is supported. Found Content-Type: {secret.Properties.ContentType}");
}
}
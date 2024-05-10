#r "nuget: Azure.Identity, 1.5.0"
#r "nuget: Azure.Security.KeyVault.Certificates, 4.2.0"

using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System.Security.Cryptography.X509Certificates;

// AKV Details
string akvUrl = @"https://<AKVUrl>.vault.azure.net/";
string certName = "Certificate Name";

// App Registration Details
string tenantId = "Tenant Id";
string clientId = "Client Id";
string clientSecret = "Client Secret";

// Create certificate client
var clientSecretCredential = new ClientSecretCredential(
    tenantId,
    clientId,
    clientSecret,
    new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud });

var certClient = new CertificateClient(new Uri(akvUrl), clientSecretCredential);
var response = certClient.DownloadCertificate(certName);

X509Certificate2? cert = response.Value;

Console.WriteLine(cert);
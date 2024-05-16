/*
 * AzureKeyVaultCertificateRetrieval.csx
 *
 * Description:
 * This script demonstrates how to retrieve a certificate and secrets from Azure Key Vault using the Azure SDK for .NET.
 * It uses client secret credentials for authentication and retrieves a specified certificate and a list of secrets.
 *
 * Prerequisites:
 * - An Azure Key Vault with the necessary certificates and secrets.
 * - An Azure AD application with client ID, client secret, and tenant ID.
 * - The Azure.Identity and Azure.Security.KeyVault.Certificates packages.
 *
 * Usage:
 * - Set the environment variables for AzureClientId, AzureClientSecret, and AzureTenantId.
 * - Replace <AKVUrl> with the URL of your Azure Key Vault.
 * - Replace "Certificate Name" with the name of the certificate you want to retrieve.
 * - Update the keysInAkv array with the names of the secrets you want to retrieve.
 */

#load "../reference.csx"

#r "nuget: Azure.Identity, *"
#r "nuget: Azure.Security.KeyVault.Certificates, *"

using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: ture, reloadOnChange: false)
    .AddEnvironmentVariables().Build();

// AKV Details
string akvUrl = @"https://<AKVUrl>.vault.azure.net/";
string certName = "Certificate Name";

// App Registration Details
var azureClientId = config["AzureClientId"];
var azureClientSecret = config["AzureClientSecret"];
var azureTenantId = config["AzureTenantId"];

// Create certificate client
var clientSecretCredential = new ClientSecretCredential(
    tenantId,
    clientId,
    clientSecret,
    new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud }
);

var certClient = new CertificateClient(new Uri(akvUrl), clientSecretCredential);
var response = certClient.DownloadCertificate(certName);

var keysInAkv = new string[]
{
    "Email-Address-stcapc-automation",
    "Email-Password-stcapc-automation",
};

foreach (var key in keysInAkv)
{
    KeyVaultSecret secret = client.GetSecret(key);
    Console.WriteLine($"Secret: {secret.Name} = {secret.Value}");
}
X509Certificate2? cert = response.Value;

Console.WriteLine(cert);

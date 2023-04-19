using Azure.Core;
using Azure.Identity;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PBIActualizador
{
    private static readonly string TenantId = "8635f36c-c46a-41d8-8ab7-8888e5e2499c";
    private static readonly string ClientId = "06109f0d-398e-4ea9-9206-5a10422275e6";
    private static readonly string ApiUrl = "https://api.powerbi.com/v1.0/";

    public async Task<bool> UpdateDataSets(List<string> datasetIds)
    {
        try
        {
            // Create a Power BI client using the InteractiveBrowserCredential.
            var credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
            {
                ClientId = ClientId,
                TenantId = TenantId
            });

            var accessToken = await credential.GetTokenAsync(new TokenRequestContext(new[] { "https://analysis.windows.net/powerbi/api/.default" }));

            var tokenCredentials = new TokenCredentials(accessToken.Token, "Bearer");

            using (var client = new PowerBIClient(new Uri(ApiUrl), tokenCredentials))
            {
                foreach (var datasetId in datasetIds)
                {
                    await client.Datasets.RefreshDatasetAsync(datasetId);
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static async Task Main(string[] args)
    {
        PBIActualizador actualizador = new PBIActualizador();
        var datasetIds = new List<string> { "a1282d75-6a9c-4efe-ad1b-bda84efd1a1e", "2718e0a8-2ab6-4de1-a1db-dcba64954433" };
        bool result = await actualizador.UpdateDataSets(datasetIds);
        Console.WriteLine("Data sets updated: " + result);
    }
}
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.ContainerRegistry;
using Pulumi.AzureNative.Storage.Inputs;
using AzureNative = Pulumi.AzureNative;

using SkuName = Pulumi.AzureNative.Storage.SkuName;
class MyStack : Stack
{
    public MyStack()

    {
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("resourceGroup");

        // Create an Azure resource (Storage Account)
        var storageAccount = new StorageAccount("sa", new StorageAccountArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuArgs
            {
                Name = AzureNative.Storage.SkuName.Standard_LRS
            },
            Kind = Kind.StorageV2
        });

        // Enable static website support
        var staticWebsite = new StorageAccountStaticWebsite("staticWebsite", new StorageAccountStaticWebsiteArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name,
            IndexDocument = "index.html",
        });

// Upload the file
var index_html = new Blob("index.html", new BlobArgs
{
    ResourceGroupName = resourceGroup.Name,
    AccountName = storageAccount.Name,
    ContainerName = staticWebsite.ContainerName,
    Source = new FileAsset("index.html"),
    ContentType = "text/html",
});

        

            // Web endpoint to the website
this.StaticEndpoint = storageAccount.PrimaryEndpoints.Apply(
        primaryEndpoints => primaryEndpoints.Web);

        // Export the primary key of the Storage Account
        this.PrimaryStorageKey = Output.Tuple(resourceGroup.Name, storageAccount.Name).Apply(names =>
            Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));




    }

    [Output]
    public Output<string> StaticEndpoint { get; set; }

    [Output]
    public Output<string> PrimaryStorageKey { get; set; }

    private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
    {
        var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
        {
            ResourceGroupName = resourceGroupName,
            AccountName = accountName
        });
        return accountKeys.Keys[0].Value;
    }

}




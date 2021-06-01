using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace BlazorImageUpload.Services
{
    public interface IBlobFactory
    {
        Task<BlobContainerClient> CreateContainerAsync(string containerName, CancellationToken cancellationToken = default);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

using Azure.Storage.Blobs;

namespace BlazorImageUpload.Services
{
    public class BlobFactory : IBlobFactory
    {
        private readonly string _connectionString;

        public BlobFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));

            _connectionString = connectionString;
        }

        public async Task<BlobContainerClient> CreateContainerAsync(string containerName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException($"'{nameof(containerName)}' cannot be null or whitespace.", nameof(containerName));

            var container = new BlobContainerClient(_connectionString, containerName);
            await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            return container;
        }
    }
}
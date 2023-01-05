using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using BlazorImageUpload.Services;
using System.IO;

namespace BlazorImageUpload.Commands
{
    public record UploadFile(Guid FileId, Stream FileStream) : IRequest;

    public class UploadFileHandler : IRequestHandler<UploadFile>
    {
        private readonly IBlobFactory _blobFactory;

        public UploadFileHandler(IBlobFactory blobFactory)
        {
            _blobFactory = blobFactory ?? throw new ArgumentNullException(nameof(blobFactory));
        }

        public async Task<Unit> Handle(UploadFile command, CancellationToken cancellationToken)
        {
            var blobName = $"file_{command.FileId}.jpg";
            var blobContainer = await _blobFactory.CreateContainerAsync("uploaded-files", cancellationToken);
            await blobContainer.DeleteBlobIfExistsAsync(blobName, cancellationToken: cancellationToken);

            if (command.FileStream is not null)
            {
                await blobContainer.UploadBlobAsync(blobName, command.FileStream, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
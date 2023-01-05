using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using BlazorImageUpload.Services;

namespace BlazorImageUpload.Commands
{
    public record UploadImage(Guid ImageId, byte[] ImageData) : IRequest;

    public class UploadImageHandler : IRequestHandler<UploadImage>
    {
        private readonly IBlobFactory _blobFactory;

        public UploadImageHandler(IBlobFactory blobFactory)
        {
            _blobFactory = blobFactory ?? throw new ArgumentNullException(nameof(blobFactory));
        }

        public async Task<Unit> Handle(UploadImage command, CancellationToken cancellationToken)
        {
            var blobName = $"image_{command.ImageId}.jpg";
            var blobContainer = await _blobFactory.CreateContainerAsync("uploaded-images", cancellationToken);
            await blobContainer.DeleteBlobIfExistsAsync(blobName, cancellationToken: cancellationToken);

            if (command.ImageData is not null)
            {
                using var ms = new System.IO.MemoryStream(command.ImageData);
                await blobContainer.UploadBlobAsync(blobName, ms, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
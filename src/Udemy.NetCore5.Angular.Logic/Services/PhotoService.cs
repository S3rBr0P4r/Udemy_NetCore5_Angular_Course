using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Udemy.NetCore5.Angular.Logic.Helpers;
using Udemy.NetCore5.Angular.Logic.Interfaces;

namespace Udemy.NetCore5.Angular.Logic.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret
            };

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParameters = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                
                uploadResult = await _cloudinary.UploadAsync(uploadParameters).ConfigureAwait(false);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParameters = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParameters).ConfigureAwait(false);
        }
    }
}
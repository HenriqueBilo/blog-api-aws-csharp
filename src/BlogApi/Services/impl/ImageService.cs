using Amazon.S3;
using Amazon.S3.Model;

namespace BlogApi.Services.impl
{
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3ClientContext;
        private readonly string _bucketName;


        public ImageService(IAmazonS3 amazonS3, IConfiguration configuration)
        {
            _s3ClientContext = amazonS3;
            _bucketName = configuration["Aws:S3:BucketName"]!;
        }

        public async Task<string> UploadAsync(IFormFile file, string postId)
        {
            var key = $"posts/{postId}/{file.FileName}";

            using var fileContent = file.OpenReadStream();

            var request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileContent,
                ContentType = file.ContentType
            };

            await _s3ClientContext.PutObjectAsync(request);

            return key;
        }
    }
}

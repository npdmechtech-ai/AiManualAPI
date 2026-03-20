using Amazon;
using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace AiManual.API.Services
{
    public class S3Service
    {
        private readonly string _bucketName = "";
        private readonly IAmazonS3 _s3Client;

        public S3Service(IConfiguration config)
        {
            var aws = config.GetSection("AWS");

            _bucketName = aws["BucketName"] ?? "";

            _s3Client = new AmazonS3Client(
                aws["AccessKey"],
                aws["SecretKey"],
                RegionEndpoint.GetBySystemName(aws["Region"])
            );
        }

        public string GetImageUrl(string key)
        {
            // Fix path mismatch
            key = key.Replace("procedure/", "Procedures/");
            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }
    }
}
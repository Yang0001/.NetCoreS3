
using System.Threading.Tasks;
using S3TestWebApi.Models;

namespace S3TestWebApi.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task UploadFIleAsync(string bucketName);
        Task DownloadObjectFromS3Async(string bucketName);
    }
}

using System.Threading.Tasks;
using S3TestWebApi.Models;

namespace S3TestWebApi.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task UploadFIleAsync(string bucketName);
        Task DownloadObjectFromS3Async(string bucketName);
        Task ListingObjectsAsync(string bucketName);
        
        Task MoveObjects(string sourceBucket, string destinationBucket);
        Task CopyObject(string sourceBucket, string objectKey, string destinationBucket, string destObjectKey);
        Task DeleteObject(string bucketName, string keyName);
        
             
        
        

    }
}
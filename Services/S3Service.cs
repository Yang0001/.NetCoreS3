using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using S3TestWebApi.Models;

namespace S3TestWebApi.Services
{
    
    public class S3Service: IS3Service
    {
        
        private readonly IAmazonS3 _client;
        IAmazonS3 client { get; set; }
        public S3Service(IAmazonS3 client)
        {           
            _client = client;            
        }


        // public S3Service()
        // {
        //      var config = new AmazonS3Config
        //     {
        //         RegionEndpoint = RegionEndpoint.APSoutheast2, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
        //         // ServiceURL = "http://localhost:9000", // replace http://localhost:9000 with URL of your minio server
        //         ForcePathStyle = true // MUST be true to work correctly with Minio server
        //     };
        //     var accessKey = "AKIAJHXSSXJP5B4AK3UQ";
        //     var secretKey = "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";
        //     _client = new AmazonS3Client(accessKey, secretKey, config);            
        // }

        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {            
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client,bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response{
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode
                    };

                }

            }
            catch (AmazonS3Exception e)
            {
               return new S3Response
               {
                   Status = e.StatusCode,
                   Message = e.Message
               };
            }


            catch (Exception e)
            {
                return new S3Response
               {
                   Status = HttpStatusCode.InternalServerError,
                   Message = e.Message
               };
            }

            return new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Something went wrong"

            };


        }



    }

    
}
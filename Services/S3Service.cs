using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using S3TestWebApi.Models;

namespace S3TestWebApi.Services
{

    public class S3Service : IS3Service
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
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);

                    return new S3Response
                    {
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

        private const string FilePath = "Startup.cs";
        private const string UploadWithName = "UploadWithName";
        private const string FileStreamUpload = "FileStreamUpload";
        private const string AdvancedUpload = "AdvanceUpload";

        public async Task UploadFIleAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);
                

                //Option1
                await fileTransferUtility.UploadAsync(FilePath, bucketName);

                //Option2
                // await fileTransferUtility.UploadAsync(FilePath, bucketName, UploadWithName);

                //Option3
                // FileStream method are not supported in Mac OS
                // using (var fileToupload = new FileStream(FilePath, bucketName.Open, UploadWithName.Read))    
                // {
                //     await fileTransferUtility.UploadAsync(fileToupload, bucketName, FileStreamUpload);
                // }

                //Option4
            //    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            //    {
            //         BucketName = bucketName,
            //         FilePath = FilePath,
            //         StorageClass = S3StorageClass.Standard,
            //         PartSize = 6291456, //6mb
            //         Key = AdvancedUpload,
            //         CannedACL = S3CannedACL.NoACL
                    
            //    };

            //    fileTransferUtilityRequest.Metadata.Add("param1","Value1");
            //    fileTransferUtilityRequest.Metadata.Add("param2","Value2");

            //    await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }

                
        }

        public async Task DownloadObjectFromS3Async(string bucketName)
        {
            const string keyName = "s3TestFile.txt";
            try
            {
               var request = new GetObjectRequest
               {
                   BucketName = bucketName,
                   Key = keyName
               };

               string responseBody;

               using (var response = await _client.GetObjectAsync(request))
               using (var responseStream = response.ResponseStream)
               using (var reader = new StreamReader(responseStream))
               {
                   var title = response.Metadata["x-amz-meta-title"];
                   var contentType = response.Headers["Content-Type"];

                   Console.WriteLine($"Object meta, Title: {title}");
                   Console.WriteLine($"Object type: {contentType}");

                   responseBody = reader.ReadToEnd();
               }

                var pathAndFileName = $"/Users/yangpu/test/{keyName}";
                var createText = responseBody;

                File.WriteAllText(pathAndFileName,createText);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
    }


}
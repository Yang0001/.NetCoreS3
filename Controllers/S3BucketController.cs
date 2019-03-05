using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S3TestWebApi.Models;
using S3TestWebApi.Services;
//dotnet add package AWSSDK.Extensions.NETCore.Setup

namespace S3TestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/S3Bucket")]
    public class S3BucketController : Controller
    {
        private readonly IS3Service _service;

        public S3BucketController(IS3Service service)
        {
            _service = service;
        }
        
        [HttpPost("{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);
            return Ok(response);
        }

        [HttpPost]        
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
            await _service.UploadFIleAsync(bucketName);
            return Ok();
        }
   
        [HttpGet]
        [Route("GetFile/{bucketName}")]
        public async Task<IActionResult> DownloadObjectFromS3Async ([FromRoute] string bucketName)
        {
            await _service.DownloadObjectFromS3Async(bucketName);
            return Ok();
        }

        [HttpGet]
        [Route("GetFileList/{bucketName}")]
        public async Task<IActionResult> ListingObjectsAsync ([FromRoute] string bucketName)
        {
            await _service.ListingObjectsAsync(bucketName);
            return Ok();
        }

        [HttpGet]
        [Route("Move")]
        public async Task<IActionResult> MoveObjects()
        {
            string Source = "upload-yang";
            string Des = "des-yang";
            await _service.MoveObjects(Source,Des);
            return Ok();
        }
       

       
    

        
    }

   
}


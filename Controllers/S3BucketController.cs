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

       
    

        
    }

   
}


﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealEstate.Application.Services;
using System;

namespace RealEstate.Infrastructure.Services
{
    public class FileService
    {
        public readonly Cloudinary cloudinary;
        public FileService(IConfiguration config)
        {
            Account account = new Account(
                config.GetSection("CloudinarySettings:CloudName").Value,
                config.GetSection("CloudinarySettings:ApiKey").Value,
                config.GetSection("CloudinarySettings:ApiSecret").Value
                );
            cloudinary = new Cloudinary( account );
        }
        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {
            var uploadResult=new ImageUploadResult();
            if(photo.Length > 0 )
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                    .Height(500).Width(800)
                };
              
                    uploadResult = await cloudinary.UploadAsync(uploadParams);
               

            }
            return uploadResult;

        }
        public async Task<CloudinaryDotNet.Actions.DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result=await cloudinary.DestroyAsync(deleteParams);
            return result;

        }
    }
}

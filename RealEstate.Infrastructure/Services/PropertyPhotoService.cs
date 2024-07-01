


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities.Property;
using RealEstate.Infrastructure.Data;

namespace RealEstate.Infrastructure.Services
{
    public class PropertyPhotoService:IPropertyPhotoService
    {
        private readonly IProperty propertyRepository;
        private readonly FileService fileService;
        private readonly AppDbContext context;

        public PropertyPhotoService(IProperty propertyRepository, FileService fileService,AppDbContext context)
        {
            this.propertyRepository = propertyRepository;
            this.fileService = fileService;
            this.context = context;
        }
        public async Task<GeneralResponse> AddPropertyPhotoAsync(IFormFile file, int propertyId)
        {
            var result = await fileService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                return new GeneralResponse(false, "Error uploading the image");
            }
            var property = await propertyRepository.GetPropertyAsync(propertyId);
            var image = new Image
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (property.Images.Count == 0)
            {
                image.IsPrimary = true;
            }
            property.Images.Add(image);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Image Uploaded");

        }
        public async Task<GeneralResponse> SetPrimaryPhotoAsync(int propertyId, string photoPublicId)
        {
            var propertyDetailDto = await propertyRepository.GetPropertyAsync(propertyId);
            if (propertyDetailDto == null)
            {
                return new GeneralResponse(false, "No such property exist");
            }
            //if (property.agent != userId)
            //{
            //    return new GeneralResponse(false, "you are not authorized to change the photo");
            //}
            var property = await context.Properties
                  .Include(p => p.Images)
                  .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
            {
                return new GeneralResponse(false, "No such property exists");
            }
            var photo = property.Images.FirstOrDefault(p => p.PublicId == photoPublicId);
            if (photo == null)
            {
                return new GeneralResponse(false, "Image does not exist");
            }
            if (photo.IsPrimary)
            {
                return new GeneralResponse(false, "This is already a primary photo");
            }

            var currentPrimary = property.Images.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null)
            {
                currentPrimary.IsPrimary = false;
            }

            photo.IsPrimary = true;

            // Ensure the context tracks changes
            context.Properties.Update(property);
            await context.SaveChangesAsync();

            return new GeneralResponse(true);


        }
        public async Task<GeneralResponse> DeletePhotoAsync(int propertyId, string photoPublicId)
        {
            try
            {
                var propertyDetailDto = await propertyRepository.GetPropertyById(propertyId);
                if (propertyDetailDto == null)
                {
                    return new GeneralResponse(false, "No such property exists");
                }
                // Uncomment and modify the authorization check if needed
                // if (property.AgentId != userId)
                // {
                //     return new GeneralResponse(false, "You are not authorized to delete the photo");
                // }

                var property = await context.Properties
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == propertyId);

                if (property == null)
                {
                    return new GeneralResponse(false, "No such property exists");
                }

                var photo = property.Images.FirstOrDefault(p => p.PublicId == photoPublicId);
                if (photo == null)
                {
                    return new GeneralResponse(false, "Image does not exist");
                }
                if (photo.IsPrimary)
                {
                    return new GeneralResponse(false, "You cannot delete the primary photo");
                }
                var result = await fileService.DeletePhotoAsync(photoPublicId);
                if (result.Error != null)
                {
                    return new GeneralResponse(false, result.Error.Message);

                }
                property.Images.Remove(photo);

                // Ensure the context tracks changes
                context.Properties.Update(property);
                await context.SaveChangesAsync();

                return new GeneralResponse(true);
            }
            catch (Exception ex)
            {
                // Return an error response with the exception message
                return new GeneralResponse(false, $"Failed to delete the photo: {ex.Message}");
            }
        }


    }
}

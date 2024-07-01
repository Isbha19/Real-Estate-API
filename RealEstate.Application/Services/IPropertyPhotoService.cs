using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
  
        public interface IPropertyPhotoService
        {
            Task<GeneralResponse> AddPropertyPhotoAsync(IFormFile file, int propertyId);
            Task<GeneralResponse> SetPrimaryPhotoAsync(int propertyId, string photoPublicId);
            Task<GeneralResponse> DeletePhotoAsync(int propertyId, string photoPublicId);
        }
    
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Infrastructure.Services;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly FileService _fileSerice;

        public DemoController(FileService fileSerice)
        {
            _fileSerice = fileSerice;
        }
        [HttpPost("add/photo/{id}")]
        [Authorize]
        public async Task<IActionResult> AddProperty(IFormFile file,int propId)
        {
            var result=await _fileSerice.UploadPhotoAsync(file);
            if(result.Error!= null)
            {
                return BadRequest(result.Error.Message);
            }
            return Ok(201);
        }
    }
}

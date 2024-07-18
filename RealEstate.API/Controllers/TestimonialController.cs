using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request;
using RealEstate.Infrastructure.Repo;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialController : ControllerBase
    {
        private readonly ITestimonial testimonial;

        public TestimonialController(ITestimonial testimonial)
        {
            this.testimonial = testimonial;
        }
        [HttpPost("submit-testimonial")]
        [Authorize]

        public async Task<IActionResult> SubmitTestimonial([FromBody] TestimonialDto testimonialDto)
        {
            var result = await testimonial.SubmitTestimonialAsync(testimonialDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("get-all-testimonials")]
        public async Task<ActionResult<IEnumerable<TestimonialDto>>> GetAllTestimonials()
        {
            var testimonials = await testimonial.GetAllTestimonialsAsync();
            return Ok(testimonials);
        }
    }
}

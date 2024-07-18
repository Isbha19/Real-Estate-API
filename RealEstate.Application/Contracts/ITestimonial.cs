using RealEstate.Application.DTOs.Request;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Testimonial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Contracts
{
    public interface ITestimonial
    {
        Task<GeneralResponse> SubmitTestimonialAsync(TestimonialDto testimonialDto);
        Task<IEnumerable<TestimonialDetailDto>> GetAllTestimonialsAsync();


    }
}

using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Testimonial;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repo
{
    public class TestimonialRepo : ITestimonial
    {
        private readonly GetUserHelper getUserHelper;
        private readonly AppDbContext context;

        public TestimonialRepo(GetUserHelper getUserHelper,AppDbContext context)
        {
            this.getUserHelper = getUserHelper;
            this.context = context;
        }
        public async Task<GeneralResponse> SubmitTestimonialAsync(TestimonialDto testimonialDto)
        {
            var user = await getUserHelper.GetUser();
            var userId = user.Id;

            var testimonial = new Testimonial
            {
                UserId = userId,
                Title = testimonialDto.Title,
                Message = testimonialDto.Message,
                Rating = testimonialDto.Rating,
                SubmittedOn = DateTime.UtcNow
            };

            context.Testimonials.Add(testimonial);
            await context.SaveChangesAsync();

            return new GeneralResponse(true, "Testimonial submitted successfully.");
        }
        public async Task<IEnumerable<TestimonialDetailDto>> GetAllTestimonialsAsync()
        {
            return await context.Testimonials
                .Include(t => t.User)
                .Select(t => new TestimonialDetailDto
                {
                    Title = t.Title,
                    Message = t.Message,
                    Rating = t.Rating,
                    UserName = t.User.FirstName + " " + t.User.LastName 
                })
                .ToListAsync();
        }
    }
}

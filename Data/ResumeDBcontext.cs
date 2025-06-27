using Microsoft.EntityFrameworkCore;
using Rest_API_CV.Models;

namespace Rest_API_CV.Data
{
    public class ResumeDBcontext : DbContext
    {
        public ResumeDBcontext(DbContextOptions<ResumeDBcontext> options) : base(options)
        {

        }
        public DbSet<Person> People { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<Education> Educations { get; set; }
    
    }
}

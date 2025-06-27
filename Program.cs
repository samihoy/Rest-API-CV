
using Microsoft.EntityFrameworkCore;
using Rest_API_CV.Data;
using Rest_API_CV.EndPoints;
using Rest_API_CV.Models;

namespace Rest_API_CV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ResumeDBcontext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHttpClient();


            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            APIEndPoints.RegisterEndPoints(app);

            // jag vet inte hur ni vill ha testdatan medskickad så jag bad chat gpt lägga till fejkdata
            // det är kåden under, vill vara klar och tydlig att jag inte skrev den uttan det är bara så du (läraren)
            // kan testa endpointsen

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ResumeDBcontext>();

                db.Database.Migrate();

                if (!db.Set<Person>().Any())
                {
                    var person = new Person
                    {
                        Name = "Max Example",
                        Description = "Example description",
                        Adress = "123 Fake Street",
                        PhoneNumber = 123456789,
                        Educations = new List<Education>
            {
                new Education
                {
                    SchoolName = "Fake University",
                    Diploma = "Computer Science",
                    EnrolmentDate = new DateTime(2015, 9, 1),
                    GraduationDate = new DateTime(2019, 6, 30)
                }
            },
                        Employments = new List<Employment>
            {
                new Employment
                {
                    JobbTitle = "Developer",
                    Company = "Code Inc.",
                    StartOfEmployment = new DateTime(2020, 1, 1),
                    EndOfEmployment = new DateTime(2022, 1, 1)
                }
            }
                    };

                    db.Add(person);
                    db.SaveChanges();
                }
            }

            app.Run();
            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rest_API_CV.Data;
using Rest_API_CV.DTO;
using Rest_API_CV.Models;
using System.Text.Json;

namespace Rest_API_CV.EndPoints
{
    public class APIEndPoints
    {
        public static void RegisterEndPoints(WebApplication app)
        {
            //------------------------------------------------------------------------------------------------------------
            //      Hämta all data (alla personer, utbildningar och jobberfarenheter).
            //------------------------------------------------------------------------------------------------------------

            app.MapGet("/alldata", async (ResumeDBcontext context) =>
            {
                var People = await context.People.Include(e => e.Educations).Include(e => e.Employments).ToArrayAsync();

                if (People.IsNullOrEmpty())
                {
                    return Results.NotFound("No data found in database");
                }

                return Results.Ok(People);
            });

            //------------------------------------------------------------------------------------------------------------
            //      Hämta en specifik post baserat på dess ID.
            //------------------------------------------------------------------------------------------------------------

            app.MapGet("/Getperson/{id}", async (ResumeDBcontext context, int id) =>
            {
                var Person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                if (Person == null)
                {
                    return Results.NotFound("No Person found in database");
                }

                return Results.Ok(Person);

            });

            //------------------------------------------------------------------------------------------------------------
            //      Lägga till ny utbildning eller jobberfarenhet.
            //      jag har skappat den så att du anger ett användar id och får lägga till en employment till den användaren
            //------------------------------------------------------------------------------------------------------------

            app.MapPost("/Add/Employment/Person/{id}", async (ResumeDBcontext context, int id, Employment employment) =>
            {
                var Person = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                if (Person == null)
                {
                    return Results.NotFound("No user in database");
                }

                Person.Employments.Add(employment);
                await context.SaveChangesAsync();
                return Results.Ok("Emplyment added to person");


            });

            //------------------------------------------------------------------------------------------------------------
            //      Uppdatera befintlig information (t.ex. ändra jobbtitel eller examensår).
            //------------------------------------------------------------------------------------------------------------


            app.MapPut("/Uppdate/Person/{id}", async (ResumeDBcontext context, int id, Person person) =>
            {
                var DBPerson = await context.People.FirstOrDefaultAsync(p => p.Id == id);

                if (DBPerson == null)
                {
                    return Results.NotFound("no user found in database");
                }

                if (DBPerson.Name != person.Name && person.Name != null)
                {
                    DBPerson.Name = person.Name;
                }
                if (DBPerson.Description != person.Description && person.Description != null)
                {
                    DBPerson.Description = person.Description;
                }
                if (DBPerson.Adress != person.Adress && person.Adress != null)
                {
                    DBPerson.Adress = person.Adress;
                }
                if (DBPerson.PhoneNumber != person.PhoneNumber && person.Adress != null)
                {
                    DBPerson.PhoneNumber = person.PhoneNumber;
                }

                await context.SaveChangesAsync();

                return Results.Ok();

            });

            //------------------------------------------------------------------------------------------------------------
            //      Ta bort en utbildning eller jobberfarenhet.
            //------------------------------------------------------------------------------------------------------------

            app.MapDelete("delete/education/{id}", async (ResumeDBcontext context, int id) =>
            {
                var education = await context.Educations.FirstOrDefaultAsync(e => e.Id == id);

                if (education == null)
                {
                    return Results.BadRequest("No education found");
                }

                context.Educations.Remove(education);
                await context.SaveChangesAsync();
                return Results.Ok("Education deleted");

            });

            //------------------------------------------------------------------------------------------------------------
            //     * Implementera en endpoint där en användare kan ange sitt GitHub-användarnamn
            //     * API: et hämta en lista över personens publika GitHub-repositories via GitHub API.
            //     * Returnera följande information: Repository-namn,Språk som används i repot(om inget språk anges, returnera “okänt” som värde).
            //                                       Beskrivning av repot(om finns, annars “saknas” som värde), Länk till repot.
            //------------------------------------------------------------------------------------------------------------

            app.MapGet("/api/github/{username}", async (string username, HttpClient httpClient) =>
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MaxResumeapp");

                var url = $"https://api.github.com/users/{username}/repos";

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return Results.NotFound("Inget github konto hittat");
                }

                var json = await response.Content.ReadAsStringAsync();

                var repos = JsonSerializer.Deserialize<List<GithubRepoDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var result = repos.Select(repo => new GithubRepoDto
                {
                    Name = repo.Name,
                    Language = string.IsNullOrEmpty(repo.Language) ? "okänt" : repo.Language,
                    Description = string.IsNullOrEmpty(repo.Description) ? "saknas" : repo.Description,
                    HtmlUrl = repo.HtmlUrl
                });

                return Results.Ok(result);
            });
        }
    }
}

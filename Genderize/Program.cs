using Genderize;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod());


app.MapGet("/api/classify", async (string name) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(name))
            return Results.BadRequest(new { status = "error", message = "name cannot be empty" });
        if (!name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            return Results.UnprocessableEntity(new { status = "error", message = "name must be a valid string" });

        var client = new HttpClient();
        JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
        client.BaseAddress = new Uri("https://api.genderize.io");
        var httpResponseMessage = await client.GetAsync($"?name={name}");
        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<GenderizeDTO>(responseContent, options);
        if (response.Gender is null || response.Count == 0)
            return Results.NotFound(new { status = "error", message = "No prediction available for the provided name" });

        var mappedResponse = new
        {
            status = "success",
            data = new ClassifyDTO
            {
                Name = response.Name,
                Gender = response.Gender,
                Probability = response.Probability,
                Sample_size = response.Count,
                Is_confident = response.Probability > 0.7,
                Processed_at = DateTime.UtcNow
            }
        };
        return Results.Ok(mappedResponse);
    }
    catch (Exception ex)
    {
        return Results.Json(new { status = "error", message = "An error has occured" }, statusCode:500);
    }
});

app.Run();



using HobbyHive.Database;
using HobbyHive.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
//builder.Services.AddAntiforgery();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        "Host=panel.sebastiankura.com;Port=25579;Database=oneparagraph;Username=hobbyhatch;Password=nalesnik",
        new MySqlServerVersion(new Version(10, 3, 39))));

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed((host) => true)
    .AllowCredentials()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/get-activities", async (GetActivitiesRequest getActivitiesRequest, DataContext dataContext) =>
    {
        var latD = 111.32 * 1000 / getActivitiesRequest.Radius;
        var lonD = getActivitiesRequest.Radius /
                   (400075 * 1000 * Math.Cos(getActivitiesRequest.Latitude * Math.PI / 180));

        return dataContext.Activities
            .Where(c =>
                c.Latitude > getActivitiesRequest.Latitude - latD
                && c.Latitude < getActivitiesRequest.Latitude + latD
                && c.Longitude > getActivitiesRequest.Longitude - lonD
                && c.Longitude < getActivitiesRequest.Longitude + lonD)
            .OrderBy(a => a
                .Date);
    })
    .WithName("GetActivities");

app.MapPost("/register-user", async (RegisterUserRequest request, DataContext dataContext) =>
{
    List<Category> categories = [];

    foreach (var id in request.CategoryIds)
        categories.Add(dataContext.Categories.FirstOrDefault(a => a.Id == id) ?? dataContext.Categories.First());

    dataContext.Users.Add(User.Create(
        request.Name,
        request.Surname,
        request.Email,
        request.Tags,
        categories));

    await dataContext.SaveChangesAsync();
});

app.MapPost("/add-activity", async (AddActivityRequest request, DataContext
        dataContext) =>
    {
        List<Category> categories = [];

        foreach (var id in request.CategoryIds)
            categories.Add(dataContext.Categories.FirstOrDefault(a => a.Id == id) ?? dataContext.Categories.First());

        var host = dataContext.Users.First(x => x.Id == request.HostId);

        dataContext.Activities.Add(
            new Activity(
                request.Title,
                request.Description,
                request.Image,
                DateTime.Parse(request.Date),
                request.Latitude,
                request.Longitude,
                request.IsInvalid,
                host,
                categories
            ));

        await dataContext.SaveChangesAsync();
    })
    .DisableAntiforgery()
    .WithName("AddActivity");

app.Run();

public class GetActivitiesRequest
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double Radius { get; set; }
}

public class RegisterUserRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<string> Tags { get; set; }
    public List<int> CategoryIds { get; set; }
}

public class AddActivityRequest()
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public string Image { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsInvalid { get; set; }
    public Guid HostId { get; set; }
    public List<int> CategoryIds { get; set; }
}
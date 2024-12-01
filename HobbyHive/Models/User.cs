using System.ComponentModel.DataAnnotations.Schema;

namespace HobbyHive.Models;

public sealed class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }

    [Column(TypeName = "JSON")]
    public List<string> Tags { get; set; } = [];
    public List<Category> Categories { get; set; } = [];
    public List<Activity> Activities { get; set; } = [];
    
    private User() {}

    public static User Create(
        string name,
        string surname,
        string email,
        IList<string> tags,
        IList<Category> categories)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            Email = email,
            Tags = [..tags],
            Categories = [..categories]
        };
    }
}
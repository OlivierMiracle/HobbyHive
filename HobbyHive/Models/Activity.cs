namespace HobbyHive.Models;

public sealed class Activity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public DateTime Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsInvalid { get; set; }
    
    public Guid HostId { get; set; }
    public List<User> Participants { get; set; }
    public List<Category> Categories { get; set; }

    public Activity() { }
    public Activity(
        string title, 
        string description, 
        string image, 
        DateTime date, 
        double latitude, 
        double longitude, 
        bool isInvalid, 
        User host, 
        List<Category> categories)
    {
        
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Image = image;
        Date = date;
        Latitude = latitude;
        Longitude = longitude;
        IsInvalid = isInvalid;
        HostId = host.Id;
        Participants = [host];
        Categories = categories;
    }
}
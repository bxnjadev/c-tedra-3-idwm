using System.ComponentModel.DataAnnotations;

namespace Catedra3.Model;

public class Post
{

    public int Id { get; set; } = 0;
    
    public string Title { get; set; } = string.Empty;

    public string DateCreated { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int UserId { get; set; } = 0;
    
    public User User { get; set; } = null;

}

public class CreationPost
{
    
    [MinLength(5)]
    public string Title { get; set; } = string.Empty;

    public string DateCreated { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int UserId { get; set; } = 0;
    
    public User User { get; set; } = null;

    
}
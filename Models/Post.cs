using System.ComponentModel.DataAnnotations;

namespace Demnoboard.Models;

public class Reply
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
}

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public virtual ICollection<Reply>? Replies{get; set; }
}
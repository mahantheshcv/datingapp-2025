using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Email { get; set; }
}

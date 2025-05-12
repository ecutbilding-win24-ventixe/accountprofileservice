using System.ComponentModel.DataAnnotations;

namespace Presentation.Entities;

public class ProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

}

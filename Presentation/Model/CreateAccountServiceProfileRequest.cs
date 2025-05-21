namespace Presentation.Model;

public class CreateAccountServiceProfileRequest
{
    public string UserId { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

namespace Business.Models.Requests;

public class UpdateAccountProfileRequest
{
    public string UserId { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
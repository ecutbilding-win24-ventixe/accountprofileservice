namespace Business.Models.Requests;

public class CreateAccountProfileRequest
{
    public string UserId { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public int AddressTypeId { get; set; }
}

namespace Business.Models.Requests;

public class CreateAddreessInfoRequest
{
    public string UserId { get; set; } = null!;
    public int AddressTypeId { get; set; }
    public string StreetName { get; set; } = null!;
    public string? StreetNumber { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}

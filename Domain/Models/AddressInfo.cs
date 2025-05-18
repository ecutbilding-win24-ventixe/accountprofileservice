namespace Domain.Models;

public class AddressInfo
{
    public string Id { get; set; } = null!;
    public string StreetName { get; set; } = null!;
    public string? StreetNumber { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public Profile UserId { get; set; } = null!;
    public AddressTypeModel AddressType { get; set; } = null!;
}

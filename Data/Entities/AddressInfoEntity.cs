using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AddressInfoEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();


    [ForeignKey(nameof(AddressType))]
    public int AddressTypeId { get; set; }
    public AddressTypeEntity AddressType { get; set; } = null!;

    public string StreetName { get; set; } = null!;
    public string? StreetNumber { get; set; }

    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}


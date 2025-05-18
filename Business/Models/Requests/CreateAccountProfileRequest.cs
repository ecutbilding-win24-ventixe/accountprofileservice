using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests;

public class CreateAccountProfileRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

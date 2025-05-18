﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

}


using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class Department
{
    public int? Id { get; set; }

    public string? Ad { get; set; } = null!;

    public string? Durum { get; set; }

    public virtual ICollection<Personel>? Personels { get; set; } = new List<Personel>();
}

using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<Personel> Personels { get; set; } = new List<Personel>();
}

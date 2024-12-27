using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class MaasPrim
{
    public int? Id { get; set; }

    public int? AdminId { get; set; }

    public int? PersonelId { get; set; }

    public int? Tutar { get; set; }

    public string? OdemeDurumu { get; set; }

    public string? Tarih { get; set; }

    public string? Not { get; set; }

    public virtual Personel? Admin { get; set; } = null!;

    public virtual Personel? Personel { get; set; } = null!;
}

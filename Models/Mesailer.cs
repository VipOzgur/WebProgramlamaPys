using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class Mesailer
{
    public int? Id { get; set; }

    public int? AdminId { get; set; }

    public int? Personel { get; set; }

    public string? StartTime { get; set; }

    public string? FinishTime { get; set; }

    public int SaatlikUcret { get; set; }

    public int? Tutar { get; set; }

    public string? OdenmeDurumu { get; set; }

    public string? Not { get; set; }

    public string? Tarih { get; set; }

    public virtual Personel? Admin { get; set; } = null!;

    public virtual Personel? PersonelNavigation { get; set; } = null!;
}

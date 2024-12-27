using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class Izinler
{
    public int? Id { get; set; }

    public int? AdminId { get; set; }

    public int? PersonelId { get; set; }

    public string? IzinTipi { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? FinishDate { get; set; }

    public string? Not { get; set; }

    public string? Tarih { get; set; }

    public virtual Personel? Admin { get; set; } = null!;

    public virtual Personel? Personel { get; set; } = null!;
}

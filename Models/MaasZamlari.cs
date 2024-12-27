using System;
using System.Collections.Generic;

namespace WebFinalPys.Models;

public partial class MaasZamlari
{
    public int? Id { get; set; }

    public int? AdminId { get; set; }

    public int? PersonelId { get; set; }

    public int? Tutar { get; set; }

    public string? Tarih { get; set; }

    public string? Aciklama { get; set; }

    public string? Yuzde { get; set; }

    public int? Tip { get; set; }

    public virtual Personel? Admin { get; set; } = null!;

    public virtual Personel? Personel { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFinalPys.Models;

public partial class Personel
{
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Adres { get; set; }

    public string Meil { get; set; } = null!;

    public int? DepId { get; set; }

    public int Durum { get; set; }

    public int? Maas { get; set; }

    public DateOnly? StartDate { get; set; }

    public string? Not { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public string? Profil { get; set; }

    public string TcNo { get; set; } = null!;

    public int? Prim { get; set; }

    public int? Mesai { get; set; }

    public virtual Department? Dep { get; set; }

    public virtual ICollection<Izinler> IzinlerAdmins { get; set; } = new List<Izinler>();

    public virtual ICollection<Izinler> IzinlerPersonels { get; set; } = new List<Izinler>();

    public virtual ICollection<MaasPrim> MaasPrimAdmins { get; set; } = new List<MaasPrim>();

    public virtual ICollection<MaasPrim> MaasPrimPersonels { get; set; } = new List<MaasPrim>();

    public virtual ICollection<MaasZamlari> MaasZamlariAdmins { get; set; } = new List<MaasZamlari>();

    public virtual ICollection<MaasZamlari> MaasZamlariPersonels { get; set; } = new List<MaasZamlari>();

    public virtual ICollection<Mesailer> MesailerAdmins { get; set; } = new List<Mesailer>();

    public virtual ICollection<Mesailer> MesailerPersonelNavigations { get; set; } = new List<Mesailer>();

    public virtual Role Role { get; set; } = null!;
}

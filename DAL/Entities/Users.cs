using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Users
    {
        public int No { get; set; }

        public string AdSoyad { get; set; }

        public string KullaniciAd { get; set; }

        public string Sifre { get; set; }

        public string Eposta { get; set; }

        public int? RolNo { get; set; }

        public virtual ICollection<Favori> Favori { get; set; } = new List<Favori>();

        public virtual ICollection<ProjectYorum> ProjectYorum { get; set; } = new List<ProjectYorum>();

        public virtual Rol RolNoNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Material
    {
        public int No { get; set; }

        public string Baslik { get; set; }

        public int? KategoriNo { get; set; }

        public virtual Kategori KategoriNoNavigation { get; set; }

        public virtual ICollection<Pm> Pm { get; set; } = new List<Pm>();

        public virtual ICollection<Project> Project { get; set; } = new List<Project>();
    }
}

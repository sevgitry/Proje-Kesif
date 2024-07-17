using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Kategori
    {
        public int No { get; set; }

        public string Ad { get; set; }

        public virtual ICollection<Material> Material { get; set; } = new List<Material>();
    }
}

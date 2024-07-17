using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Project
    {
        public int No { get; set; }

        public string Ad { get; set; }

        public string Explain { get; set; }

        public int? MaterialNo { get; set; }

        public int? TurNo { get; set; }

        public virtual ICollection<Favori> Favori { get; set; } = new List<Favori>();

        public virtual Material MaterialNoNavigation { get; set; }

        public virtual ICollection<Pm> Pm { get; set; } = new List<Pm>();

        public virtual ICollection<ProjectYorum> ProjectYorum { get; set; } = new List<ProjectYorum>();

        public virtual Tur TurNoNavigation { get; set; }
    }
}

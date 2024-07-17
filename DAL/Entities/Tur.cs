using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Tur
    {
        public int No { get; set; }

        public string Ad { get; set; }

        public virtual ICollection<Project> Project { get; set; } = new List<Project>();
    }
}

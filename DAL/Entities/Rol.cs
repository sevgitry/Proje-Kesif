using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Rol
    {
        public int No { get; set; }

        public string Ad { get; set; }

        public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    }
}

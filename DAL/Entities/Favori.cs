using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Favori
    {
        public int No { get; set; }

        public int? ProjectNo { get; set; }

        public int? UsersNo { get; set; }

        public DateTime? EklemeTarih { get; set; }

        public virtual Project ProjectNoNavigation { get; set; }

        public virtual Users UsersNoNavigation { get; set; }
    }
}

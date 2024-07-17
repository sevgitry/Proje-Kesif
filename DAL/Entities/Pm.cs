using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class Pm
    {
        public int No { get; set; }

        public int? ProjectNo { get; set; }

        public int? MaterialNo { get; set; }

        public virtual Material MaterialNoNavigation { get; set; }

        public virtual Project ProjectNoNavigation { get; set; }
    }
}

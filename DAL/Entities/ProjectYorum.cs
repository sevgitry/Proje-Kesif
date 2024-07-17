using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public partial class ProjectYorum
    {
        public int No { get; set; }

        public string Konu { get; set; }

        public string Aciklama { get; set; }

        public int? ProjectNo { get; set; }

        public int? UsersNo { get; set; }

        public virtual Project ProjectNoNavigation { get; set; }

        public virtual Users UsersNoNavigation { get; set; }
    }
}

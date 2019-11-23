using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkoutGen.Models
{
    public partial class AspNetUserLogins
    {
        public int Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}

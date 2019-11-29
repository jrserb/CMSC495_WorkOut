﻿using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class AspNetUserTokens
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}

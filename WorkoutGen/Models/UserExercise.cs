using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserExercise
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}

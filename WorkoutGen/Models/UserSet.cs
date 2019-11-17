using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserSet
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }
        public string Repetitions { get; set; }
        public string Weight { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}

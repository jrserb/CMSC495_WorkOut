using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class ExerciseMuscleGroup
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int MuscleGroupId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual MuscleGroup MuscleGroup { get; set; }
    }
}

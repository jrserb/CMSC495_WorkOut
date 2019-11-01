using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class MuscleGroup
    {
        public MuscleGroup()
        {
            ExerciseMuscleGroup = new HashSet<ExerciseMuscleGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
    }
}

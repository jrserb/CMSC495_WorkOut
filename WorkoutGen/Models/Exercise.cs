using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class Exercise
    {
        public Exercise()
        {
            ExerciseEquipment = new HashSet<ExerciseEquipment>();
            ExerciseMuscleGroup = new HashSet<ExerciseMuscleGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseEquipment> ExerciseEquipment { get; set; }
        public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
    }
}

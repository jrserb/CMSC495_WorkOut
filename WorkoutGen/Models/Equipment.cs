using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class Equipment
    {
        public Equipment()
        {
            ExerciseAlternateEquipment = new HashSet<ExerciseAlternateEquipment>();
            ExerciseEquipment = new HashSet<ExerciseEquipment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseAlternateEquipment> ExerciseAlternateEquipment { get; set; }
        public virtual ICollection<ExerciseEquipment> ExerciseEquipment { get; set; }
    }
}

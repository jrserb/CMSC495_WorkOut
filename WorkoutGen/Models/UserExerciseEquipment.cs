using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserExerciseEquipment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ExerciseId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}

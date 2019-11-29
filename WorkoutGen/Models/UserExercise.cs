using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class UserExercise
    {
        public UserExercise()
        {
            UserExerciseEquipment = new HashSet<UserExerciseEquipment>();
            UserExerciseMuscleGroup = new HashSet<UserExerciseMuscleGroup>();
            UserSet = new HashSet<UserSet>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<UserExerciseEquipment> UserExerciseEquipment { get; set; }
        public virtual ICollection<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}

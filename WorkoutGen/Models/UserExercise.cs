using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Hyperlink { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<UserExerciseEquipment> UserExerciseEquipment { get; set; }
        public virtual ICollection<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}

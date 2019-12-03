using Microsoft.AspNetCore.Identity;
using System;

namespace WorkoutGen.Models
{
    public partial class UserExerciseMuscleGroup
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int UserExerciseId { get; set; }
        public int MuscleGroupId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual MuscleGroup MuscleGroup { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual UserExercise UserExercise { get; set; }
    }
}

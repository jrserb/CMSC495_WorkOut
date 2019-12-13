/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model user object
*/

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkoutGen.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserEquipmentSet = new HashSet<UserEquipmentSet>();
            UserExercise = new HashSet<UserExercise>();
            UserExerciseEquipment = new HashSet<UserExerciseEquipment>();
            UserExerciseMuscleGroup = new HashSet<UserExerciseMuscleGroup>();
            UserWorkout = new HashSet<UserWorkout>();
        }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<UserEquipmentSet> UserEquipmentSet { get; set; }
        public virtual ICollection<UserExercise> UserExercise { get; set; }
        public virtual ICollection<UserExerciseEquipment> UserExerciseEquipment { get; set; }
        public virtual ICollection<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserWorkout> UserWorkout { get; set; }
    }
}

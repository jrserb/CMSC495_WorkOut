/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model user exercise object
*/

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
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [MaxLength(250)]
        public string Hyperlink { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<UserExerciseEquipment> UserExerciseEquipment { get; set; }
        public virtual ICollection<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}

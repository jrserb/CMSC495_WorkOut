/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model exercise object
*/

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
            UserSet = new HashSet<UserSet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hyperlink { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseEquipment> ExerciseEquipment { get; set; }
        public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}

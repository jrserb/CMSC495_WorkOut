/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model muscle group object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class MuscleGroup
    {
        public MuscleGroup()
        {
            ExerciseMuscleGroup = new HashSet<ExerciseMuscleGroup>();
            UserExerciseMuscleGroup = new HashSet<UserExerciseMuscleGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
        public virtual ICollection<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
    }
}

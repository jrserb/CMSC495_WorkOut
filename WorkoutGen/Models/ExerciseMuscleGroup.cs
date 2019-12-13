/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model exercise muscle group object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class ExerciseMuscleGroup
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int MuscleGroupId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual MuscleGroup MuscleGroup { get; set; }
    }
}

/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model user set object
*/

using System;

namespace WorkoutGen.Models
{
    public partial class UserSet
    {
        public int Id { get; set; }
        public int? ExerciseId { get; set; }
        public int? UserExerciseId { get; set; }
        public int UserWorkoutId { get; set; }
        public string Repetitions { get; set; }
        public string Weight { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Exercise Exercise { get; set; }
        public virtual UserExercise UserExercise { get; set; }
        public virtual UserWorkout UserWorkout { get; set; }
    }
}

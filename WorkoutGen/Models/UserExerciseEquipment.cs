/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model exercise equipment object
*/

using System;

namespace WorkoutGen.Models
{
    public partial class UserExerciseEquipment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int UserExerciseId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual UserExercise UserExercise { get; set; }
    }
}

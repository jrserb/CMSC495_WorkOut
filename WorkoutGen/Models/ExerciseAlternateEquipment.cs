/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model exercise alternate equipment object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class ExerciseAlternateEquipment
    {
        public int Id { get; set; }
        public int ExerciseEquipmentId { get; set; }
        public int AlternateEquipmentId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Equipment AlternateEquipment { get; set; }
        public virtual ExerciseEquipment ExerciseEquipment { get; set; }
    }
}

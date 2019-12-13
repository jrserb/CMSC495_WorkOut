/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model exercise equipment object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class ExerciseEquipment
    {
        public ExerciseEquipment()
        {
            ExerciseAlternateEquipment = new HashSet<ExerciseAlternateEquipment>();
        }

        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual ICollection<ExerciseAlternateEquipment> ExerciseAlternateEquipment { get; set; }
    }
}

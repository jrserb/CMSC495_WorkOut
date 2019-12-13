/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model equipment object
*/

using System;
using System.Collections.Generic;

namespace WorkoutGen.Models
{
    public partial class Equipment
    {
        public Equipment()
        {
            ExerciseAlternateEquipment = new HashSet<ExerciseAlternateEquipment>();
            ExerciseEquipment = new HashSet<ExerciseEquipment>();
            UserEquipmentSetEquipment = new HashSet<UserEquipmentSetEquipment>();
            UserExerciseEquipment = new HashSet<UserExerciseEquipment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual ICollection<ExerciseAlternateEquipment> ExerciseAlternateEquipment { get; set; }
        public virtual ICollection<ExerciseEquipment> ExerciseEquipment { get; set; }
        public virtual ICollection<UserEquipmentSetEquipment> UserEquipmentSetEquipment { get; set; }
        public virtual ICollection<UserExerciseEquipment> UserExerciseEquipment { get; set; }
    }
}

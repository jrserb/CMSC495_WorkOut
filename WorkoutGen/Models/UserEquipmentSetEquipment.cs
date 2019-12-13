/*
    Name: Brett Snyder
    Date: 12/12/2019
    Course: CMSC 495 - Current Trends And Projects in Computer Science
    Desc: Model equipment set equipment object
*/

using System;

namespace WorkoutGen.Models
{
    public partial class UserEquipmentSetEquipment
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int UserEquipmentSetId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateDeleted { get; set; }

        public virtual Equipment Equipment { get; set; }
    }
}
